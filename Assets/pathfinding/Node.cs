using UnityEngine;
using System.Collections;

//Grid node, and heap item(for a more efficient implementation of the a* algorithm).
public class Node : IHeapItem<Node> {

	//Is this node an obstacle or part of a clear path?.
	public bool walkable;

	//Center of the node.
	public Vector2 worldPosition;
	//Position on the grid.
	public int gridX;
	public int gridY;

	//Variables for A* algorithm(pending a* implementation details).
	public int gCost;
	public int hCost;

	//Heap implementation variables.
	public Node parent;

	int heapIndex;

	//Node Constructor.
	public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY) {
		//Variable initialization.
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	
	}

	//calculation of the f cost.
	public int fCost {
		get {
			return gCost + hCost;
		}
	}
	//heap index property
	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}
	//implementation of the compare interface for use in the heap sorting.
	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}



}
