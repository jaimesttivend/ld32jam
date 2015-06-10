using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;


public class Pathfinding : MonoBehaviour {
	
	//Object in charge of asking and managing pathfinding tasks.
	PathRequestManager requestManager;

	//grid of nodes 
	Grid grid;

	//initialization of variables (must be attached to the same GameObject).
	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}

	//class implementation of giving the order to the grid to update itself.
	public void updateGrid(){
		grid.Updates ();
	}

	/// <summary>
	/// Method to start the find path corroutine.
	/// </summary>
	/// <param name="startPos">Start position.</param>
	/// <param name="targetPos">Target position.</param>
	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos,targetPos));
	}

	//Coroutine implemented for parallel (and consecutive?) pathFinding. 
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
		//Stopwatch to measure the time it takes for the algorithm to find the best path.
		Stopwatch sw = new Stopwatch();
		//Start of the stopwatch.
		sw.Start();

		//list of positions containing the points that form the best path.
		Vector3[] waypoints = new Vector3[0];

		bool pathSuccess = false;
		//Getting the node corresponding to the unit's start searching position.
		Node startNode = grid.NodeFromWorldPoint(startPos);

		//Getting the node corresponding to the unit's target position.
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		//The start node and  the target node must be walkable.
		if (startNode.walkable && targetNode.walkable) {

			//OpenSet heap for a* algorithm optimization.
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			//closed set for A* algorithm implementation.
			HashSet<Node> closedSet = new HashSet<Node>();
			//A* algorithm implementation
			openSet.Add(startNode);

			while (openSet.Count > 0) {
				/*in the first iteration it will empty the heap and then it will just sent the currrent?
				node(and first node in the heap? ) to the closed set*/
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				//If the algorithm has arrived to a solution.
				if (currentNode == targetNode) {
					//Stop the stopwatch
					sw.Stop();
					//Register this time on the console
					print ("Path found: " + sw.ElapsedMilliseconds + " ms");
					//signal that the routine has arrived to the solution.
					pathSuccess = true;
					//Go outside the while loops.
					break;

				}
				//for each  walkable neighbour to the current node that is not in the closed set.
				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {

					if (!neighbour.walkable || closedSet.Contains(neighbour)) {
						continue;
					}

					//calculates the cgcost of that nehigbour if added to the open set  
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

					/*if the cost of moving from the current node to the neighbour is less than its current gcost,
					*or the node is not already in the openset
					*/
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {

						//update or asigns the node's gcost, gcost and asign the current node as parent
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						//if the node is not in the openset, it adds it. 
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
		}
		//waits for a change of frame.
		yield return null;

		//verifies if the path was found and sends the signal and result to the requestmanager
		if (pathSuccess) {
			//recreates a path of nodes given the starting and target node
			waypoints = RetracePath(startNode,targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);

	}


	//Coroutine implemented for parallel (and consecutive?) pathFinding. 
	public void FindPathNonCoroutine(Vector3 startPos, Vector3 targetPos) {

		//list of positions containing the points that form the best path.
		Vector3[] waypoints = new Vector3[0];
		
		bool pathSuccess = false;
		//Getting the node corresponding to the unit's start searching position.
		Node startNode = grid.NodeFromWorldPoint(startPos);
		
		//Getting the node corresponding to the unit's target position.
		Node targetNode = grid.NodeFromWorldPoint(targetPos);
		
		//The start node and  the target node must be walkable.
		if (startNode.walkable && targetNode.walkable) {
			
			//OpenSet heap for a* algorithm optimization.
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			//closed set for A* algorithm implementation.
			HashSet<Node> closedSet = new HashSet<Node>();
			//A* algorithm implementation
			openSet.Add(startNode);
			
			while (openSet.Count > 0) {
				/*in the first iteration it will empty the heap and then it will just sent the currrent?
				node(and first node in the heap? ) to the closed set*/
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);
				
				//If the algorithm has arrived to a solution.
				if (currentNode == targetNode) {

					pathSuccess = true;
					//Go outside the while loops.
					break;
					
				}
				//for each  walkable neighbour to the current node that is not in the closed set.
				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					
					if (!neighbour.walkable || closedSet.Contains(neighbour)) {
						continue;
					}
					
					//calculates the cgcost of that nehigbour if added to the open set  
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					
					/*if the cost of moving from the current node to the neighbour is less than its current gcost,
					*or the node is not already in the openset
					*/
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						
						//update or asigns the node's gcost, gcost and asign the current node as parent
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						//if the node is not in the openset, it adds it. 
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
		}

		//verifies if the path was found and sends the signal and result to the requestmanager
		if (pathSuccess) {
			//recreates a path of nodes given the starting and target node
			waypoints = RetracePath(startNode,targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);
		
	}



	//Given 2 nodes after a shorter path has been found between the two giving a list on point that compose the 
	//path
	Vector3[] RetracePath(Node startNode, Node endNode) {
		//list of nodes intended to return.
		List<Node> path = new List<Node>();

		//Starting with the last node treated with the a* algorithm (target or end node).
		Node currentNode = endNode;

		//Add the nodes to the path  and follow with its parent 
		//node until the start node is found.
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		/*Convert the list of nodes into a list of points 
		 *( at this point it should just do that but it can
		 *also smooth the path with static objectives and 
		 *diagonal movement.
		
		*/
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;

	}
	/* simplify eliminating redundant nodes diagonals
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	*/

	//converts a list of nodes to a path.
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		//Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			//Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			//if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			//}
			//directionOld = directionNew;
		}
		return waypoints.ToArray();
	}


	/*Function for calculating the distance to a  node giving diagonal 
	 * steps a distance of 14 and other nodes disntance of 10, also cero to itself.
	 * 
	 */
	int GetDistance(Node nodeA, Node nodeB) {
		//get distance on x and y
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		//to work without diagonals. Comment if working with diagonals
		return dstX * 10 + dstY * 10;

		/* uncomment to work with diagonals
		// if the neightbouring node is vertical(dstx = 0)
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		// if the neightbouring node is horizontal(dstx = 0) or diagonal dtsx = dsty
		return 14*dstX + 10 * (dstY-dstX);*/
	}


}
