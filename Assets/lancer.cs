﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using enums;

public enum directions{
	
	up,
	down ,
	right,
	left
	
}


public class lancer : MonoBehaviour {

	public bool active = false;
	public directions directiones = directions.up;
	public directions direction = directions.right;
	public Vector3 translation;
	public float gaitVelocity;
	public float chazeVelocity;
	public float velocity;
	public GameObject target;
	public int damage;

	public bool killer;

	//public Image tail;
	public Text tex;
	public Image bubB;
	public Image bub;


	// Use this for initialization
	void Start () {

		translation = new Vector3 (0f, 0f, 0f);

	}




	// Update is called once per frame
	void Update () {
	
		if (target == null) {
			velocity = gaitVelocity * Time.deltaTime;
		
			if(direction==directions.up){
				translation.y = velocity;
					
					
			}else{
				translation.y = -velocity;
					
			}


		} else {

			velocity = chazeVelocity * Time.deltaTime;
			translation =(target.transform.position- transform.position);

			translation=translation.normalized*velocity;
	
		
		}




		transform.Translate(translation);



	}




	void OnCollisionEnter2D(Collision2D coll){

		if (direction == directions.up) {
			direction = directions.down;
		} else {
			direction= directions.up;
		}



	}

	public void atention(){
		bubB.enabled = false;
		bub.enabled = true;
		tex.text=" !!! ";


	}

	void OnTriggerEnter2D (Collider2D col){


		if (col.gameObject == target) {

			player p = (player)col.GetComponent (typeof(player));
			if (p != null) {

				p.activatedamash (transform.position, damage);
			}
		} else {
		
			Flower f =(Flower)col.GetComponent (typeof(Flower));

			if(f != null){

				if(f.type==1){
					if(!killer)
						target=f.transform.gameObject;

					tex.text="So pretty";
				}

			}
		
		}



	}
	
}
