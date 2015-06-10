using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerUnit :Unit {

	private bool traced;		//specifies that the path has been traced.
	public GameObject arrow;
	public List<GameObject> arrows ;
	public bool drawnPath;//* too complex.


	/// <summary>
	/// Initialize  PlayerUnit.
	/// </summary>
	void Start () {

		//arrow = (GameObject)Resources.Load ("gridmovement/prefabs/arrow0000.prefab");

		//var initialization.
		this.target = this.transform.position;
		this.path = null;

		this.anim = GetComponent<Animator> ();

	}

	//verify which parts of the father need to be changed ?
	// Update is called once per frame
	void Update () {

		/*if (target != null && traced ==false){



		}*/

	}

	/// <summary>
	/// Sets the target.
	/// and 
	/// </summary>
	/// <param name="TargetNode">Target node.</param>
	public void SetTarget(int x, int y){
		Node TargetNode = gird.grid [x, y];//note: this should be restricted to only numbers in a particular range
		target = TargetNode.worldPosition;
		request ();
	}

	/// <summary>
	/// sets the path to the target, if a path is found
	/// </summary>
	/// <param name="newPath">New path.</param>
	/// <param name="pathSuccessful">If set to true path successful it will plot the way trough it</param>
	public override void OnPathFound (Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful) {
			path = newPath;
			drawPath ();
			drawnPath=true;
		} else {
			drawnPath=false;
		
		}
	}

	/// <summary>
	/// Draws the path once it's found.
	/// </summary>
	public void drawPath(){
		//clears the arrows of the late paths drawn
		clearArrows ();

		GameObject recentArrow = null; //the last arrow that went trough the loop

		if (path != null) {

			foreach (Vector3 position in path){

				if(recentArrow!=null){//verifying this is not the firts iteration.
					int direction =4;
					direction=getdirection(recentArrow.transform.position,position);

					switch(direction){
					case 0:
						recentArrow.transform.rotation=  Quaternion.Euler(0,0,90);
						break;
					
					case 1:
						recentArrow.transform.rotation=  Quaternion.Euler(0,0,270);
						break;
					case 3:
						recentArrow.transform.rotation=  Quaternion.Euler(0,0,180);
						break;
					}
				}

				GameObject arrowInst= Instantiate(arrow);
				arrowInst.transform.position= position;
				arrowInst.transform.Translate(0f,0f,1f);
				arrows.Add (arrowInst);
				recentArrow=arrowInst;

				}


			   if(recentArrow!=null){
				int direction =4;
				direction=getdirection(recentArrow.transform.position,target);
				
				switch(direction){
				case 0:
					recentArrow.transform.rotation=  Quaternion.Euler(0,0,90);
					break;
					
				case 1:
					recentArrow.transform.rotation=  Quaternion.Euler(0,0,270);
					break;
				case 3:
					recentArrow.transform.rotation=  Quaternion.Euler(0,0,180);
					break;
				}
			}
			}
			
		}



	/// <summary>
	/// clears the arrow with the current set path to the target.
	/// </summary>
	public void clearArrows(){

		foreach (GameObject arrow in arrows) {
			Destroy(arrow);
		}
		arrows.Clear ();
	}
	/// <summary>
	/// What to do when arriving at A waypoint.
	/// </summary>
	public override void whatToDoWhenArrivingToAWaypoint ()
	{
		if (targetIndex < arrows.Count)
			Destroy (arrows [targetIndex]);
		else
			arrows.Clear ();
	}

	/// <summary>
	/// makes a request for the  for the path manager and executes the method onPathFound accordingly
	/// </summary>
	protected virtual void request(){
		
		PathRequestManager.RequestPathNonCorutine(transform.position,target, OnPathFound);
		
	}



}