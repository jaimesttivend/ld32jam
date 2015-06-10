using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {


	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();		//Queue of path requests.

	PathRequest currentPathRequest;		//Current path request
	static PathRequestManager instance;	//Static instance
	Pathfinding pathfinding;			//Pathfinding object reference
	bool isProcessingPath;				//State variable for (busy, not busy)

	//Initialization method.
	void Awake() {
		//Singleton implementation.
		instance = this;

		//Getting the pathfinder Game Object
		pathfinding = GetComponent<Pathfinding>();
	}

	//note: A very rought way of making the grid update.
	public static void pfgu(){
	
		instance.pathfinding.updateGrid ();
	}

	/// <summary>
	/// Finds the requested path and saves? a callback method.
	/// </summary>
	/// <param name="pathStart">Path start.</param>
	/// <param name="pathEnd">Path end.</param>
	/// <param name="callback">Callback method.</param>
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
		//Create a new path request.
		PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
		//Add path request to Queue
		instance.pathRequestQueue.Enqueue(newRequest);

		instance.TryProcessNext();
	}


	/// <summary>
	/// Finds the requested path and saves? a callback method.
	/// </summary>
	/// <param name="pathStart">Path start.</param>
	/// <param name="pathEnd">Path end.</param>
	/// <param name="callback">Callback method.</param>
	public static void RequestPathNonCorutine(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
		//Create a new path request.
		PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
		//Add path request to Queue
		instance.currentPathRequest = newRequest;
		instance.pathfinding.FindPathNonCoroutine(newRequest.pathStart, newRequest.pathEnd);

	}


	void TryProcessNext() {
		// if there are no paths being processed and there are paths left in the queue.
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			//Get the last path of the queue and set it as the current path.
			currentPathRequest = pathRequestQueue.Dequeue();
			// Signals itself as busy
			isProcessingPath = true;

			//starts the pathfinding process
			pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}
	/// <summary>
	/// Writes the resulting path and the success value on the request ,
	/// and tries to process the next request
	/// </summary>
	/// <param name="path">Path.</param>
	/// <param name="success">If set to <c>true</c> success.</param>
	public void FinishedProcessingPath(Vector3[] path, bool success) {

		currentPathRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();

	}

	///Path request structure
	struct PathRequest {

		public Vector3 pathStart;	//Starting Gloval point
		public Vector3 pathEnd;		//target Gloval point
		public Action<Vector3[], bool> callback; //callback?

		/// <summary>
		/// Initializes a new instance of the <see cref="PathRequestManager+PathRequest"/> struct.
		/// </summary>
		/// <param name="_start">_start.</param>
		/// <param name="_end">_end.</param>
		/// <param name="_callback">_callback.</param>
		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}

	}
}
