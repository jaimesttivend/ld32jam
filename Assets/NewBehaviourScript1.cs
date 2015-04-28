using UnityEngine;
using System.Collections;

public class NewBehaviourScript1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void restart(){

		Application.LoadLevel("main");

	}

	public void exit(){
		
		Application.Quit();
		
	}

}
