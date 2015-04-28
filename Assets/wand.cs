using UnityEngine;
using System.Collections;

public class wand : MonoBehaviour {


	public bool active=false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D col){

		Flower fl;

		if (active) {
		
			fl=(Flower)col.GetComponent(typeof(Flower));

			if (fl != null){

				fl.activate();

			}
		
		}
	
	
	}

}
