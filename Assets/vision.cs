using UnityEngine;
using System.Collections;

public class vision : MonoBehaviour {


	public lancer  lanza;
	// Use this for initialization
	void Start () {
		lanza = (lancer)this.GetComponentInParent(typeof(lancer));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){

		player pleya;

		pleya = (player)col.GetComponent (typeof(player));

		if (pleya != null) {
		
			lanza.target = pleya.gameObject;

			lanza.atention();
			Destroy (transform.gameObject);
		}


	
	}



}
