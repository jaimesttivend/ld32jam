using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class moveGrid : MonoBehaviour {

	public GameObject nodePref;

	public int heigth;
	public int length;

	public playerUnit anthousai;
	
	public moveGridNode selectedNode;

	public bool moving ;//controls whether the unit is currently accepting follow order 

	//node instantiaion and initialization.
	void Start () {

		moving = false;

		///creating a group of nodes as wide and tall as specified.
		for (int i=0; i<length; i++) {
			for (int j=0; j<heigth; j++) {

				GameObject temp;
				moveGridNode node;
				temp =(GameObject)Instantiate(nodePref,new Vector3(i*16f+8f,j*16f+8f,+10f),Quaternion.identity);
				node =temp.GetComponent<moveGridNode>();
				node.instantiate(i,j,this);
			}
		}

	}

	public void interact(moveGridNode node){
	
		if (!anthousai.moving) {

			if (selectedNode == null) { //no node selected

				if (anthousai.gird.grid [node.x, node.y].walkable) {//selected node is walkable

					anthousai.SetTarget (node.x, node.y);

					if (anthousai.drawnPath) {

						selectedNode = node;

					} else {

						//message node not reachable.

					}

				} else {

					//message: node not selectable.

				}

		
			} else {//node already selected
		
				if (node == selectedNode) {

					anthousai.follow ();
					selectedNode = null;

				} else {

					
					if (anthousai.gird.grid [node.x, node.y].walkable) {//selected node is walkable
						
						anthousai.SetTarget (node.x, node.y);
						
						if (anthousai.drawnPath) {
							
							selectedNode = node;
							
						} else {
							
							//message node not reachable.
							
						}



					}
		
				}
			}
	
		} 
	}
	/// <summary>
	/// selects a node on the grid node.
	/// returns
	/// </summary>
	/// <param name="node">Node.</param>
	public bool setSelectedNode(moveGridNode node){

		bool success= false;
		//this should initialy switch between switch and move. not right now though.

		if (!anthousai.moving && anthousai.gird.grid [node.x, node.y].walkable) {


			anthousai.SetTarget (node.x, node.y);

			if (anthousai.path != null) {

				if (selectedNode != null) {// if a node is already selected , unselect it.
					selectedNode.unselect ();
				}

				selectedNode = node;
				success = true;
			}else{

			}

		} else {
			selectedNode = null;

		}

		return success;
	}
	/////////////////////WRONG
	/// 
	/// <summary>
	/// Unselects the node.
	/// </summary>
	/// <returns><c>true</c>, if node was unselected, <c>false</c> otherwise.</returns>
	/// <param name="node">Node.</param>
	public bool unselectNode(moveGridNode node){
		bool success;
		Node nod =anthousai.gird.NodeFromWorldPoint(anthousai.target);
		if (!anthousai.moving && node==selectedNode && nod.gridX== node.x && nod.gridY== node.y) {

			success = true;

		} else {
		
			success = false;

		}

		return success;
	}


	// Update is called once per frame
	void Update () {
	
	}
}
