using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
public class moveGridNode : MonoBehaviour {

	public int x;
	public int y;

	Vector3 position;

	public Transform backgroudnBit;
	public Transform foregroundBit;

	SpriteRenderer srBackgroundBit;
	SpriteRenderer srForegroundBit;

	bool selected=false;
	bool instantiated=false;

	moveGrid grid;

	// Use this for initialization
	void Start () {
		srBackgroundBit = backgroudnBit.GetComponent<SpriteRenderer> ();
		srForegroundBit =foregroundBit.GetComponent<SpriteRenderer> ();
		srBackgroundBit.color = Color.white;
		srForegroundBit.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
 	
	public void instantiate (int x, int y, moveGrid grid ){

		this.grid = grid;
		if (grid != null) {

			this.x = x;
			this.y = y;

			instantiated = true;
		}
	}

	public void interact(){
		grid.interact (this);
		
		/*//wrong i
		if (instantiated) {

			if (selected) {
				//unselect();
				selected= !grid.unselectNode(this);
			} else {
				//select();
				selected=grid.setSelectedNode(this);

			}
		}
		*/
	}
	/// <summary>
	/// for buton click graphical feedback 
	/// </summary>
	public void select(){

		srBackgroundBit.color = Color.red;
		srForegroundBit.color = Color.cyan;

	}
	/// <summary>
	/// for buton unclick graphical feedback 
	/// </summary>
	public void unselect(){

		srBackgroundBit.color = Color.white;
		srForegroundBit.color = Color.black;
		
	}




}
