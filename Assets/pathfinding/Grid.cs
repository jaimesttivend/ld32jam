using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// original code written by 


public class Grid : MonoBehaviour {

	public bool displayGridGizmos;// if the gizmos will be displayed on the editor
	public LayerMask unwalkableMask; //that which cannot be traversed
	public Vector2 gridWorldSize;// size of the grid set on the editor
	public float nodeRadius; //half of the width of a node
	public Node[,] grid;//nodest that compose the grid 



	float nodeDiameter; // double the node radius
	int gridSizeX, gridSizeY;// both dimentions of the gridsize

	/// <summary>
	/// Grid initialization.
	/// </summary>
	void Awake() {
		//getting the diameter from the radius
		nodeDiameter = nodeRadius*2;
		//getthing the number of nodes from the size horizontal size
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		//getthing the number of nodes from the size vertical size
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		//initialize the grid.
		CreateGrid(); 

	}

	//number of nodes of the grid based on the dimentions of the matrix created
	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	/// <summary>
	/// Sets the characteristics of the grid according to its current variables.
	/// </summary>
	public void Updates(){
		// note: i did this so it will be called whenever necesary. It's basically the create grid function but uptdtes the grid instead.
		//locates the bottom left corner 
		Vector2 worldBottomLeft = new Vector2(transform.position.x,transform.position.y) - Vector2.right * gridWorldSize.x/2 - Vector2.up * gridWorldSize.y/2;
		// for each posible  position  starting with the 0,0 going trough the values of x and y as dimentions.
		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				// Calculate  a global central position to the node 
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);

				//verify if the position of the node is unwalkable.
				bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y].walkable=walkable;//setting the propietry to the node.

			}
		}
	}

	//
	void CreateGrid() {
		//initialize the node grid with the specified size
		grid = new Node[gridSizeX,gridSizeY];

		//locates the bottom left corner 
		Vector2 worldBottomLeft = new Vector2(transform.position.x,transform.position.y) - Vector2.right * gridWorldSize.x/2 - Vector2.up * gridWorldSize.y/2;

		// for each posible  position  starting with the 0,0 going trough the values of x and y as dimentions.
		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				// Calculate  a global central position to the node 
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
				//verify if the position of the node is unwalkable.
				bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeRadius-1f,unwalkableMask));
				//Create the node.
				grid[x,y] = new Node(walkable,worldPoint, x,y);
			
			}
		}
	}

	//getting the adyacent nodes to a certain node
	public List<Node> GetNeighbours(Node node) {
		// list of nodes to return
		List<Node> neighbours = new List<Node>();

		//for all adyacent nodes 
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				//excluding the center and the corners (remove second condition of the or|| allowing corners if needed )
				if ((x == 0 && y == 0)||x==y||x==-y)
					continue;

				//caluculate the x And y values of the neighbour nodes 
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				//verify if those values are posible ( doesn't go out of the grid eg x=-1)
				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					// adds to the returning list.
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}

	// returns the position of a node in the grid given a global location vector of a point
	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		//"moving" the position vector of the element to take the center of the grid as the origin.
		worldPosition = worldPosition - transform.position;

		// calculates which percentage of the grid represents the positions of the element
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;

		//restricts it to an consistent percentage value (giving as result edge locations when the element is outside the grid)
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		//multiply for the discrete size of the greed to find the position of the node in the grid.
		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

		return grid[x,y];

	}


	//draws the nodes as an sqare grid of gizmos for easier editor debuging and comprehension.
	void OnDrawGizmos() {

		//draws the limits of the grid
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));

		//draw gizmos if the grid is initializated and according to preferences
		if (grid != null && displayGridGizmos) {


			foreach (Node n in grid) {
				//initialize walkable and unwalkable colors and alphas.
				Color walkable = Color.white;
				walkable.a=0.3f;

				Color unwalkable = Color.red;
				unwalkable.a=0.3f;

				//set color according to the nodes values
				Gizmos.color = (n.walkable)?walkable:unwalkable;
				//draw the node
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.6f));
			}
		}
	}
}