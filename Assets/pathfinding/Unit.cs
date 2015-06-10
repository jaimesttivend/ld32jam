using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {


	public Vector3 target; 	// Element followed by the unit.  
	public float speed = 20f; 	// Unit's speed
	public Vector3[] path;				// Current path to be followed
	protected int targetIndex;			// current state of moving trough the waypoints?.
	public Grid gird;			// Grid asigned to the unit.

	public bool moving=false;// allows to Know if the unit is mooving.

	public Animator anim;		//animator component of the unit

	/// <summary>
	/// Initialize unit.
	/// </summary>
	void Start() {

		request ();
		anim = GetComponent<Animator> ();

	}


	/// <summary>
	/// makes a request for the  for the path manager and executes the method onPathFound accordingly
	/// </summary>
	protected virtual void request(){
		
		PathRequestManager.RequestPath(transform.position,target, OnPathFound);

	}

	/// <summary>
	/// Starts the movement trough the found path if found
	/// </summary>
	/// <param name="newPath">New path.</param>
	/// <param name="pathSuccessful">If set to true path successful it will move trough it</param>
	public virtual void OnPathFound(Vector3[] newPath, bool pathSuccessful) {

		if (pathSuccessful) {
			path = newPath;
			StopCoroutine(FollowPath() );//stop following a non updated path.
			StartCoroutine(FollowPath());
		}

	}

	public void follow(){

		StopCoroutine(FollowPath() );//stop following a non updated path.
		StartCoroutine(FollowPath());
	
	}
	/// <summary>
	/// Corroutine  for following the optimized path
	/// </summary>
	/// <returns>The path.</returns>
	IEnumerator FollowPath() {

		Vector3[] temp;
		temp = new Vector3[path.Length+1];

		path.CopyTo (temp, 0);
		temp [path.Length] = target;

		path = temp;

		//path.add (target);


		if (path.Length > 0) {// The path must have at leat one step 
			moving=true;//movement starts.
			Vector3 currentWaypoint = path [0]; //Start with the first step
			targetIndex = 0;
			while (true) {

				if (transform.position == currentWaypoint) {//when the waypoint is reached by the unit.

					//gird.Updates ();
					//request ();

					//did i comment this so that it will update after finding the next index in the list of nodes 
					// it will update the path and grid .... it think i did.
					//if not commented this would allow the path to continue being followed 'til the end. Note: maybe i'll need to implement 2 versions of this unit meanwhile.
					//targetIndex = 0;
					whatToDoWhenArrivingToAWaypoint();
					targetIndex++; 

					if (targetIndex >= path.Length) {
						moving=false;//Movement ends.
						path=null;//clearing the path.
						yield break;

					}
					//changes the current waypoint to the next
					currentWaypoint = path [targetIndex];
				
				}
				//Changes the animation according with the direction of the movement.

				int num=4; //Idle by default

				num = getdirection(transform.position,currentWaypoint); //compares the current position and the waypoint's position to get the direction.
				if(num != 4){
					
					anim.SetInteger("direction",num);
				}

				//advances towards the current waypoint.
				transform.position = Vector3.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);

				yield return null;//new WaitForSeconds(0.2f); 

			}

		}
	}

	/// <summary>
	/// Raises the draw gizmos event.
	/// painting a line along the current path to follow.
	/// </summary>
	public void OnDrawGizmos() {
		//there must be a path
		if (path != null) {

			//for each section of the path
			for (int i = targetIndex; i < path.Length; i ++) {

				Gizmos.color = Color.black;

				Gizmos.DrawCube(path[i], Vector3.one);//draw a cube on the waypoints position

				// if the next waypoint is the one next to the unit, draw a line from the unit to the waytpoint 
				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {//draw a line from a past waypoint to the next.

					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
	/// <summary>
	/// Getdirection  resulting form moving from the start point and finish.
	/// works with diagonals but only outputs integers acording to up, down left and right.
	/// 0=up, 1= down, 2=right ,3= left
	/// </summary>
	/// <param name="start">starting point </param>
	/// <param name="finish">Finishing point</param>
	/// <returns> int 0=up, 1= down, 2=right ,3= left , 4 = start==finish </returns>
	protected int getdirection(Vector3 start, Vector3 finish){

		Vector3 result;
		int num = 4;
		result = finish - start;

		if (result.x > 0 && Mathf.Abs (result.x) > Mathf.Abs (result.y)) {
			num = 2;

		
		}
		if (result.x < 0 && Mathf.Abs (result.x) > Mathf.Abs (result.y)) {
			num = 3;
			
		}

		if (result.y > 0 && Mathf.Abs (result.y) > Mathf.Abs (result.x)) {
			num = 0;
			
		}
		if (result.y < 0 && Mathf.Abs (result.y) > Mathf.Abs (result.x)) {
			num = 1;
			
		}

		return num;
	}


	public virtual void whatToDoWhenArrivingToAWaypoint(){





	}

}
