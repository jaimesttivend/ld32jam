using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		StartCoroutine(start());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator start(){

		yield return new WaitForSeconds(4f);
		Application.LoadLevel("main");

	}

}
