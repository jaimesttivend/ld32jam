using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using options;
using enums;


#region Fsm  enums
// commands to be recieved by the finite state Machine
public enum Command {

	up,
	right,
	left,
	down,
	A,
	B,
	Start,
	select,
	il,
	ir,
	stopB,
	noInput
}

//estates if the finite state machine
public enum State{

	idleDown,
	idleRight,
	idleLeft,
	idleUp,
	movingUp,
	movingDown,
	movingLeft,
	movingRight,

	actionUp,
	actionDown,
	actionLeft,
	actionRight,

	seccondActionUp,
	seccondActiondown,
	seccondActionRight,
	seccondActionleft


}



#endregion

public class player : MonoBehaviour {
	public float tpress;

	public bool pup=false;
	public bool pdwn=false;
	public bool prght=false;
	public bool plft=false;

	public bool xp=false;
	public bool zp=false;
	public bool ap=false;
	public bool sp=false;
	

	public Text plus;

	public State stat;

	public int life=10;

	public Image[] lifes ;

	public int d;

	public Image imageA;
	public Image imageB;
	public Text textA;
	public Text textB;

	public Text tDaisies;
	public Text tblue;
	public Text tred;
	public int direction = 1;
	public sideGrab thingInfront;

	public Flower flow;


	public Dictionary< Command, int[]> commandInputMatrix;

	public Transform flowPref;

	public Transform[] transforms ;

	public Image[] selected; 

	public Vector3 damageposition;
	public bool damage=false;

	public int[] quantities;
	public int indTrans=0;

	public int daisies = 10;
	public  Process MBM;
	public  Command command ;
	public float InsSpeed;
	private float speed;
	public bool awoken= false;
	private Animator anim;	
	public GameObject left;
	public GameObject right;
	public GameObject up;
	public GameObject down;

	public Transform wleft;
	public Transform wright;
	public Transform wup;
	public Transform wdown;


	public RaycastHit2D castLeft;
	public RaycastHit2D castUp;
	public RaycastHit2D castDown;
	public RaycastHit2D castRight;

	#region virtual buttons
	void haptictouch(){
		//Vibration2.Vibrate(15L);
	
	}


	public void pupd(){
		//haptictouch ();
		pup = true;
	}

	public void pupu(){

		pup = false;
	}


	public void prghtd(){
		//haptictouch ();
		prght = true;
	}



	public void prghtu(){
		
		prght = false;
	}


	public void pdwnd(){
		//haptictouch ();
		pdwn = true;
	}
	
	public void pdwnu(){
		
		pdwn = false;
	}


	public void plftd(){
		//haptictouch ();
		plft = true;
	}

	public void plftu(){
		
		plft = false;
	}

	#region pressed




	public void launch_xpd(){
		//haptictouch ();
		StartCoroutine(xpd ());
	}

	public IEnumerator xpd(){

		xp = true;
		yield return new WaitForSeconds (0f);
		//xp = false;
	}


	public void launch_zpd(){
		//haptictouch ();
		StartCoroutine(zpd ());
	}
	
	public IEnumerator zpd(){
		
		zp = true;
		//yield return new WaitForSeconds (tpress);
		yield return new WaitForSeconds (0f);

	}


	public void launch_apd(){
		//haptictouch ();
		StartCoroutine(apd ());
	}
	
	public IEnumerator apd(){
		
		ap = true;
		yield return new WaitForSeconds (0f);
		//ap = false;
	}


	public void launch_spd(){
		//haptictouch ();
		StartCoroutine(spd ());
	}
	
	public IEnumerator spd(){
		
		sp = true;
		yield return new WaitForSeconds (0f);
		//sp = false;
	}


	
	#endregion 
	#endregion

	void LateUpdate(){
		//transform.position = new Vector3 (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y), Mathf.RoundToInt (transform.position.z)); 
	}

	// Use this for initialization
	void Awake() {

		tpress = Time.deltaTime;
		commandInputMatrix = new Dictionary< Command, int[]> { 															
			//		input								left 		right 		up 		down 		z			x	a	s
			{	Command.	down,		new int[] {		2	,		2	,		0	,	1	,		0	,		0,	0,	0}	},
			{	Command.	up,			new int[] {		2	,		2	,		1	,	0	,		0	,		0,	0,	0}	},
			{	Command.	left,		new int[] {		1	,		0	,		2	,	2	,		0	,		0,	0,	0}	},
			{	Command.	right,		new int[] {		0	,		1	,		2	,	2	,		0	,		0,	0,	0}	},
			{	Command.	il,			new int[] {		2	,		2	,		2	,	2	,		2	,		2,	1,	0}	},
			{	Command.	ir,			new int[] {		2	,		2	,		2	,	2	,		2	,		2,	0,	1}	},
			{	Command.	A,			new int[] {		2   ,		2	,		2	,	2	,		1	,		0,	0,	0}	},
			{	Command.	B,			new int[] {		2	,		2	,		2	,	2	,		0	,		1,	0,	0}	},

		};

		MBM = new Process ();
		anim = GetComponent<Animator>();// gets the player gameobject animator 
		awoken = true;
	}

	void Start(){
		quantities=new int[]{1,0,10};
		selected[0].enabled=true;
		selected[1].enabled=false;
		selected[2].enabled=false;


		wup = transform.FindChild ("flowUp");
		wdown = transform.FindChild ("flowdown");
		wright = transform.FindChild ("flowright");
		wleft = transform.FindChild ("flowleft");

		/*lifes [0] = (Image)transform.Find ("hf1");
		lifes [1] = (Image)transform.Find ("hf2");
		lifes [2] = (Image)transform.Find ("hf3");
		lifes [3] = (Image)transform.Find ("hf4");
		lifes [4] = (Image)transform.Find ("hf5");
		lifes [5] = (Image)transform.Find ("hf6");
		lifes [6] = (Image)transform.Find ("hf7");
		lifes [7] = (Image)transform.Find ("hf8");
		lifes [8] = (Image)transform.Find ("hf9");
		lifes [9] = (Image)transform.Find ("hf10");*/



	}
	public sideGrab getThingInfront(){

		sideGrab front = null;

		sideGrab upsideg = (sideGrab)up.GetComponent(typeof(sideGrab));
		sideGrab downsideg = (sideGrab)down.GetComponent(typeof(sideGrab));
		sideGrab rightsideg = (sideGrab)right.GetComponent(typeof(sideGrab));
		sideGrab leftsideg = (sideGrab)left.GetComponent(typeof(sideGrab));

		switch (direction) {
		case 0:
			front=upsideg;
			front.activate();
			downsideg.deactivate();
			rightsideg.deactivate();
			leftsideg.deactivate();
			

			break;
		
		case 1:
			front=downsideg;
			front.activate();
			upsideg.deactivate();
			rightsideg.deactivate();
			leftsideg.deactivate();
			break;
		case 2:
			front=rightsideg;
			front.activate();
			downsideg.deactivate();
			upsideg.deactivate();
			leftsideg.deactivate();
			break;
		case 3:
			front=leftsideg;
			front.activate();
			downsideg.deactivate();
			rightsideg.deactivate();
			upsideg.deactivate();
			break;
			
		}

		return front;



	}

	// Update is called once per frame
	void Update () {
		tpress = 0.000001f;

		life = quantities[2];
		if (awoken) {

			if(life<1){

				StartCoroutine(deash());


			}

			foreach (Image lifex   in lifes){

				lifex.enabled=false;

			}

			if(life<11){
				for (int i = 0 ;i<life;i++){

					lifes[i].enabled=true;

				}
				plus.enabled=false;
			}else{
				foreach (Image lifex   in lifes){
					
					lifex.enabled=true;
					
				}

				plus.enabled=true;
			}



			command = getCommandFromMatrix (inputToCommands ());

			zp=false;
			xp=false;
			ap=false;
			sp=false;
			if(command== Command.il || command== Command.ir){

				if(command==Command.il){	
					if(indTrans==1){
						indTrans=0;
					}else if(indTrans==0){
						indTrans=2;

					}else if(indTrans==2){
						indTrans=1;
						
					}

				}else if(command==Command.ir){	
					if(indTrans==1){
						indTrans=2;
					}else if(indTrans==0){
						indTrans=1;
						
					}else if(indTrans==2){
						indTrans=0;
						
					}
					
				}
			
				selected[0].enabled=false;
				selected[1].enabled=false;
				selected[2].enabled=false;


				selected[indTrans].enabled=true;

				command=Command.noInput;
			}

			flowPref = transforms[indTrans];

		
			castUp = Physics2D.Linecast (up.transform.position, transform.position, 1 << LayerMask.NameToLayer ("wall"));
			castDown = Physics2D.Linecast (down.transform.position, transform.position, 1 << LayerMask.NameToLayer ("wall"));
			castRight = Physics2D.Linecast (right.transform.position, transform.position, 1 << LayerMask.NameToLayer ("wall"));
			castLeft = Physics2D.Linecast (left.transform.position, transform.position, 1 << LayerMask.NameToLayer ("wall"));


			thingInfront=getThingInfront();

			if(thingInfront.thing==things.nothing){
				textA.text="plant";
				textB.text="";
				flow=null;
			}else if (thingInfront.thing==things.flower){

				textA.text="Pick";
				textB.text="";
				flow=thingInfront.Flow;
			}else if (thingInfront.thing==things.wall){
				
				textA.text="";
				textB.text="";
				flow=null;
			}

			tDaisies.text = quantities[0].ToString();
			tblue.text = quantities[1].ToString();
			tred.text = quantities[2].ToString();
		try {
			MBM.MoveNext(command);
		}catch( Exception e){
		}  

			stat= MBM.CurrentState;
			speed =InsSpeed;
		

		switch (MBM.CurrentState) {
		
				#region idle		
			case State.idleUp:
				stop ();
				anim.SetInteger("direction",0);
				direction=0;
			break;
		case State.idleDown:
				stop ();
				anim.SetInteger("direction",1);
				direction=1;
			break;
		case State.idleRight:
				stop ();
				anim.SetInteger("direction",2);
				direction=2;
			break;
		case State.idleLeft :
				stop ();
				anim.SetInteger("direction",3);
				direction=3;
			break;
	#endregion
				#region moving
		case State.movingUp:
				move ();
				if(!castUp){
			
					transform.Translate(0,speed*Time.deltaTime,0);}
			
				break;
		case State.movingDown:
				move ();
				if (!castDown){
				
				transform.Translate(0,-speed*Time.deltaTime,0);
				}
			break;
		case State.movingLeft:
				move ();
				if (!castLeft){
				
					transform.Translate(-speed*Time.deltaTime,0,0);}
			break;
		case State.movingRight :
				move ();

				if (!castRight){
				
						transform.Translate(speed*Time.deltaTime,0,0);}
			break;
	#endregion
				#region action
		case State.actionUp :

				putflower(0);

				break;
			case State.actionDown :
				putflower(1);
				
				break;
			case State.actionLeft :
				putflower(3);
				
				break;
			case State.actionRight :
				putflower(2);
				
				break;
			case State.seccondActionUp :
				
				StartCoroutine(wand (0));
				
				break;
			case State.seccondActiondown :
				StartCoroutine(wand (1));
				
				break;
			case State.seccondActionleft :
				StartCoroutine(wand (3));
				
				break;
			case State.seccondActionRight :
				StartCoroutine(wand (2));
				
				break;
				#endregion
			}
	


		}


	}

	IEnumerator wand(int direction){
	

		SpriteRenderer spr;
		wand fl;


		switch (direction) {
		case 0:
			spr =(SpriteRenderer)wup.GetComponent(typeof(SpriteRenderer));
			fl =(wand)wup.GetComponent(typeof(wand));
			fl.active=true;
			spr.enabled=true;

			break;

		case 1:
			//wdown.transform.enabled = true;
			spr =(SpriteRenderer)wdown.GetComponent(typeof(SpriteRenderer));
			spr.enabled=true;
			fl =(wand)wdown.GetComponent(typeof(wand));
			fl.active=true;
			break;
		case 2:
			//wright.transform.enabled=true;
			spr =(SpriteRenderer)wright.GetComponent(typeof(SpriteRenderer));
			spr.enabled=true;
			fl =(wand)wright.GetComponent(typeof(wand));
			fl.active=true;
			break;
		case 3:
			//wleft.transform.enabled=true;
			spr =(SpriteRenderer)wleft.GetComponent(typeof(SpriteRenderer));
			spr.enabled=true;
			fl =(wand)wleft.GetComponent(typeof(wand));
			fl.active=true;
			break;
		}



		yield return new WaitForSeconds(0.2f);


		try{
			MBM.MoveNext (Command.stopB);
			
		}catch(Exception e){
			IDictionary x= e.Data;
			
			
		}

		switch (direction) {
		case 0:
			spr =(SpriteRenderer)wup.GetComponent(typeof(SpriteRenderer));
			spr.enabled=false;
			fl =(wand)wup.GetComponent(typeof(wand));
			fl.active=false;
			break;
			
		case 1:
			//wdown.transform.enabled = true;
			spr =(SpriteRenderer)wdown.GetComponent(typeof(SpriteRenderer));
			spr.enabled=false;
			fl =(wand)wdown.GetComponent(typeof(wand));
			fl.active=false;
			break;
		case 2:
			//wright.transform.enabled=true;
			spr =(SpriteRenderer)wright.GetComponent(typeof(SpriteRenderer));
			spr.enabled=false;
			fl =(wand)wright.GetComponent(typeof(wand));
			fl.active=false;
			break;
		case 3:
			//wleft.transform.enabled=true;
			spr =(SpriteRenderer)wleft.GetComponent(typeof(SpriteRenderer));
			spr.enabled=false;
			fl =(wand)wleft.GetComponent(typeof(wand));
			fl.active=false;
			break;
		}

	}


	#region movement
	public void move(){
	
		anim.SetBool("moving",true);
	
	}

	public void stop(){
		
		anim.SetBool("moving",false);
		
	}

	#endregion

	void OnCollisionEnter2D(Collision2D coll)
	{
		speed = 0f;
		stop ();
		//if(col.gameObject.Equals(paddle)) 
		//	Vibration.Vibrate (12L);


		lancer lanz;
		
		
		lanz = (lancer)coll.gameObject.GetComponent(typeof(lancer));
		
		if (lanz != null) {
			
			damageposition = coll.transform.position;
			d=lanz.damage;
			damage = true;

		}
		
	}



	void putflower(int direction){
		Vector3 position = transform.position;



		float distance=16;

		switch (direction) {
		case 0:
			position = new Vector3(position.x,position.y+distance-8f,0f);


			break;
		case 1:
			position = new Vector3(position.x,position.y-distance-2f,0f);
			break;
		case 2:
			position = new Vector3(position.x+distance,position.y,0f);
			break;
		case 3:
			position = new Vector3(position.x-distance,position.y,0f);
			break;
		
		}

		if (thingInfront.thing == things.nothing) {

			if (quantities[indTrans] > 0){
				Instantiate (flowPref, position, left.transform.rotation);
				quantities[indTrans]-=1;			
			}
		
		} else if (thingInfront.thing == things.flower) {

			quantities[thingInfront.types]+=1;	

			thingInfront.destroyFlower();

			flow=null;
		}
	}


	public void ActivatedThingy(){
	
	
	
	
	}

	#region input related helping methods
		
		//gets a command enum that fits with an input vector
		public Command getCommandFromMatrix(int[] vector){
			Command command = Command.noInput;
			
			foreach(KeyValuePair<Command, int[]> entry in commandInputMatrix)
			{
				if( compareInputArrays (vector,entry.Value)){
					command = entry.Key;
					break;
				}
			}
			
			//if (command == Command.backJump || command == Command.forwardJump || command == Command.jump) 		
			//				command = Command.noInput;
			
			return command;
			
		} 
		
		//compares a input array with a the input array patter corresponding to an input command
		public bool compareInputArrays(int[] input ,int[] pattern){
			
			bool comp = true;

			
			for (int count = 0; count< input.Length; count++) {
				
				if (input[count] != pattern[count] && pattern[count] !=2  ){
					comp=false;
					break;
				}
				
			}
			return comp;
		}
		
		// creates an input Array out of the inputs recieved
		public int[] inputToCommands(){

			//	mesage	/input		left 	right 	up 	down 	z 		x	
			int[] input = new int[]{0,		0,		0,	0,		0		,0 ,0,0	}; 
			
		if (Input.GetButton ("left")||plft) {
			
			input[0]=1;	
			
		}
		
		if (Input.GetButton ("right")||prght) {
			
			input[1]=1;		
		}

			if (Input.GetButton ("up") || pup) {
				input[2]=1;		
			}
			
			if (Input.GetButton ("down")||pdwn) {
				input[3]=1;		
			}
			


		if (Input.GetButtonDown ("Z")||zp) {
			input[4]=1;		
		}
		if (Input.GetButtonDown ("x")||xp) {
			input[5]=1;		
		}


		if (Input.GetButtonDown ("il")||ap) {
			input[6]=1;		
		}
		if (Input.GetButtonDown ("ir")||sp) {
			input[7]=1;		
		}


			return input;
			
		}
		

		
		
		#endregion

	void FixedUpdate(){
		
		if (damage) {


			damage=false;

			activatedamash(damageposition,d);





		}
	}

	public void activatedamash(Vector3 vdamageposition,int dam){
		Rigidbody2D rb;
		rb= (Rigidbody2D)this.GetComponent(typeof(Rigidbody2D));
		rb.AddForce((transform.position-vdamageposition)*10f);
		life = life - dam;
		quantities [2] = life; 
		StartCoroutine(damash ());
	}


	IEnumerator  damash(){
		SpriteRenderer sr;
		Color r = new Color (100f, 0f, 0f);
		Color w = new Color (255f, 255f, 255f);
		sr=(SpriteRenderer)this.GetComponent(typeof(SpriteRenderer));
		sr.color =r;
		yield return new WaitForSeconds(0.05f);
		sr.color=w;

		yield return new WaitForSeconds(0.05f);
		sr.color =r;
		yield return new WaitForSeconds(0.05f);
		sr.color=w;

	}


	IEnumerator  deash(){
		SpriteRenderer sr2;



		sr2=(SpriteRenderer)this.GetComponent(typeof(SpriteRenderer));
		for (int i =0; i<20; i++) {
			Color a=sr2.color;
			a.a-= 0.05f;
			sr2.color=a; 
		
		yield return null;
		}
		Application.LoadLevel("gameo");
	}



		





	
}

#region Fsm Class


//class that represents the finite state machine  and its transitions.
public class Process
{
	//inner clas transitions. its a very complicated thingamajig ?
	class StateTransition
	{
		
		readonly State CurrentState; 
		readonly Command Command;
		
		public StateTransition(State currentState, Command command)
		{
			CurrentState = currentState;
			Command = command;
		}
		//its a very complicated thingamajig ?
		public override int GetHashCode()
		{
			return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			StateTransition other = obj as StateTransition;
			return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
		}
	}
	
	Dictionary<StateTransition, State> transitions;
	public State CurrentState { get; private set; }
	
	public Process()
	{
		//stating the initial state
		CurrentState = State.idleDown;
		//stating all the posible transitions on the fsm
		transitions = new Dictionary<StateTransition, State>
		{
			#region idleUp

			{ new StateTransition(State.idleUp, Command.up), State.movingUp},
			{ new StateTransition(State.idleUp, Command.right), State.idleRight},
			{ new StateTransition(State.idleUp, Command.left), State.idleLeft},
			{ new StateTransition(State.idleUp, Command.down), State.idleDown},
			{ new StateTransition(State.idleUp, Command.A), State.actionUp},
			{ new StateTransition(State.idleUp, Command.B), State.seccondActionUp},
			#endregion

			#region idledown
			
			{ new StateTransition(State.idleDown, Command.up), State.idleUp},
			{ new StateTransition(State.idleDown, Command.right), State.idleRight},
			{ new StateTransition(State.idleDown, Command.left), State.idleLeft},
			{ new StateTransition(State.idleDown, Command.down), State.movingDown},

			{ new StateTransition(State.idleDown, Command.A), State.actionDown},
			{ new StateTransition(State.idleDown, Command.B), State.seccondActiondown},
			#endregion

			#region idleleft
			
			{ new StateTransition(State.idleLeft, Command.up), State.idleUp},
			{ new StateTransition(State.idleLeft, Command.right), State.idleRight},
			{ new StateTransition(State.idleLeft, Command.left), State.movingLeft},
			{ new StateTransition(State.idleLeft, Command.down), State.idleDown},
			{ new StateTransition(State.idleLeft, Command.A), State.actionLeft},
			{ new StateTransition(State.idleLeft, Command.B), State.seccondActionleft},
			#endregion

			#region idleright
			{ new StateTransition(State.idleRight, Command.right), State.movingRight},
			{ new StateTransition(State.idleRight, Command.up), State.idleUp},

			{ new StateTransition(State.idleRight, Command.left), State.idleLeft},
			{ new StateTransition(State.idleRight, Command.down), State.idleDown},
			{ new StateTransition(State.idleRight, Command.A), State.actionRight},
			{ new StateTransition(State.idleRight, Command.B), State.seccondActionRight},
			#endregion


			#region movingUp
			
			{ new StateTransition(State.movingUp, Command.up), State.movingUp},
			{ new StateTransition(State.movingUp, Command.right), State.idleUp},
			{ new StateTransition(State.movingUp, Command.left), State.idleUp},
			{ new StateTransition(State.movingUp, Command.down), State.idleUp},
			{ new StateTransition(State.movingUp, Command.noInput), State.idleUp},
			{ new StateTransition(State.movingUp, Command.A), State.actionUp},
			{ new StateTransition(State.movingUp, Command.B), State.seccondActionUp},
			#endregion

			#region movingdown
			
			{ new StateTransition(State.movingDown, Command.up), State.idleDown},
			{ new StateTransition(State.movingDown, Command.right), State.idleDown},
			{ new StateTransition(State.movingDown, Command.left), State.idleDown},
			{ new StateTransition(State.movingDown, Command.down), State.movingDown},
			{ new StateTransition(State.movingDown, Command.noInput), State.idleDown},
			{ new StateTransition(State.movingDown, Command.A), State.actionDown},
			{ new StateTransition(State.movingDown, Command.B), State.seccondActiondown},
			#endregion
			
			
			#region movingleft
			
			{ new StateTransition(State.movingLeft, Command.up), State.idleLeft},
			{ new StateTransition(State.movingLeft, Command.right), State.idleLeft},
			{ new StateTransition(State.movingLeft, Command.left), State.movingLeft},
			{ new StateTransition(State.movingLeft, Command.down), State.idleLeft},
			{ new StateTransition(State.movingLeft, Command.noInput), State.idleLeft},
			{ new StateTransition(State.movingLeft, Command.A), State.actionLeft},
			{ new StateTransition(State.movingLeft, Command.B), State.seccondActionleft},
			#endregion
			
			#region movingright
			
			{ new StateTransition(State.movingRight, Command.up), State.idleRight},
			{ new StateTransition(State.movingRight, Command.right), State.movingRight},
			{ new StateTransition(State.movingRight, Command.left), State.idleRight},
			{ new StateTransition(State.movingRight, Command.down), State.idleRight},
			{ new StateTransition(State.movingRight, Command.noInput), State.idleRight},
			{ new StateTransition(State.movingRight, Command.A), State.actionRight},
			{ new StateTransition(State.movingRight, Command.B), State.seccondActionRight},
			#endregion

			#region actiondown
			
			{ new StateTransition(State.actionUp, Command.noInput), State.idleUp},
			{ new StateTransition(State.actionDown, Command.noInput), State.idleDown},
			{ new StateTransition(State.actionRight, Command.noInput), State.idleRight},
			{ new StateTransition(State.actionLeft, Command.noInput), State.idleLeft},


			{ new StateTransition(State.actionUp, Command.up), State.movingUp},
			{ new StateTransition(State.actionDown, Command.down), State.movingDown},
			{ new StateTransition(State.actionRight, Command.right), State.movingRight},
			{ new StateTransition(State.actionLeft, Command.left), State.movingLeft},


			#endregion


			#region actiondown

			{ new StateTransition(State.seccondActionUp, Command.noInput), State.seccondActionUp},
			{ new StateTransition(State.seccondActiondown, Command.noInput), State.seccondActiondown},
			{ new StateTransition(State.seccondActionRight, Command.noInput), State.seccondActionRight},
			{ new StateTransition(State.seccondActionleft, Command.noInput), State.seccondActionleft},


			{ new StateTransition(State.seccondActionUp, Command.stopB), State.idleUp},
			{ new StateTransition(State.seccondActiondown, Command.stopB), State.idleDown},
			{ new StateTransition(State.seccondActionRight, Command.stopB), State.idleRight},
			{ new StateTransition(State.seccondActionleft, Command.stopB), State.idleLeft},
			
			#endregion

		};
	}
	//allows to get the next state without moving to it
	public State GetNext(Command command)
	{
		StateTransition transition = new StateTransition(CurrentState, command);
		State nextState;
		if (!transitions.TryGetValue(transition, out nextState))
			throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
		return nextState;
	}
	//gets the nex state, then moves to it .
	public State MoveNext(Command command)
	{
		CurrentState = GetNext(command);
		return CurrentState;
	}




}



#endregion

