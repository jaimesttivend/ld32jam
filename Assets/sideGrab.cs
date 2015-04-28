using UnityEngine;
using System.Collections;
using System;
using enums;


public class sideGrab : MonoBehaviour {

	public bool HasFlower=false;

	public Flower Flow = null;

	public player pleya;

	public things thing;

	public bool active;

	public int types=0;

	public npc NP;

	// Use this for initialization
	void Start () {

		pleya = (player)this.GetComponentInParent(typeof(player));

		thing = things.nothing;


	}



	// Update is called once per frame
	void Update () {
	
	}

	public void destroyFlower(){
		
		Flow.destroyer ();
		thing = things.nothing;
	}

	void OnTriggerStay2D(Collider2D col){

		if (active) {
			bool catchable = false;
			Flower fl;
			wall wal;
			npc np;

			try {

				fl = (Flower)col.GetComponent (typeof(Flower));
				np=(npc)col.GetComponent(typeof(npc));

				wal = (wall)col.GetComponent (typeof(wall));

				if (wal != null) {
					thing = things.wall;
				}
			
			
				if (fl != null) {
					thing = things.flower;
					Flow = fl;
					fl.select ();

					types=fl.type;

				}
				if(np!=null){
					NP=np;
					thing=things.npc;
					np.activate();
				}

			} catch (Exception e) {

				thing = things.nothing;

			}


		}


	}

	public void activate(){
	
		active = true;

	}
 	
	public void deactivate(){
		if(Flow!=null)
			Flow.unselect();
		if (NP != null) 
			
			NP.deactivate();
			


		Flow = null;
		active = false;
		thing = things.nothing;
		NP = null;
	}

	void OnTriggerExit2D(Collider2D col){
		thing=things.nothing;
		if (Flow != null) {
			Flow.unselect();
		}
		if (NP != null) {
		
			NP.deactivate();
			NP=null;
		}
	}

}
