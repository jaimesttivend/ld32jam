using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {

	SpriteRenderer sr;
	Color blue;
	Color red; 
	Color white;
	SpriteRenderer indicator;
	SpriteRenderer twink;
	bool active =false;

	public int type =0;

	// Use this for initialization
	void Start () {
	
		indicator = (SpriteRenderer)transform.GetChild(0).GetComponent(typeof(SpriteRenderer));
		twink =(SpriteRenderer)transform.GetChild(1).GetComponent(typeof(SpriteRenderer));
		sr = (SpriteRenderer)GetComponent(typeof(SpriteRenderer) );
		red = new Color (0f, 255f, 255f);
		blue = new Color (100f, 0f, 200f);
		white = new Color (255f, 255f, 255f);
		indicator.enabled = false;
		twink.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void select(){
		//sr.color = blue;
		indicator.enabled = true;

	}

	
	public void unselect(){
		//sr.color = white;
		indicator.enabled = false;
	}
	public void activate(){

		twink.enabled = true;


		if (active == false) {

			if (type== 0||type==2) {
		
				transform.localScale=new Vector3(2f,2f,0f);

			}

		}
		active = true;
	}



	public void destroyer(){

		Destroy (transform.gameObject);
	}
}
