using UnityEngine;
using System.Collections;

using UnityEngine.UI;
public class npc : MonoBehaviour {


	public Image dialogueBox;
	public string Dialogues;
	Text thetext;
	// Use this for initialization
	void Start () {
	
		dialogueBox.enabled = false;
		thetext =(Text)dialogueBox.GetComponentInChildren (typeof(Text));
		thetext.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	public void deactivate(){

		if (dialogueBox.enabled) {
			dialogueBox.enabled = false;
			thetext.enabled = false;
		}
	}

	
	public void activate(){
		if (!dialogueBox.enabled) {
			dialogueBox.enabled = true;
			thetext.enabled = true;
			thetext.text=Dialogues;
		}
	}

}
