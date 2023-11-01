﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]
public class PlayerMovementScript : MonoBehaviour {
	Rigidbody rb;
	AudioSource source;

	[Tooltip("Current players speed")]
	public float currentSpeed;
	[Tooltip("Assign players camera here")]
	public Transform cameraMain;
	[Tooltip("Force that moves player into jump")]
	public float jumpForce = 500;
	[Tooltip("Position of the camera inside the player")]
	[HideInInspector]public Vector3 cameraPosition;

	
	private void Start(){
		source = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
		//bulletSpawn = cameraMain.Find ("BulletSpawn").transform;
		//ignoreLayer = 1 << LayerMask.NameToLayer ("Player");
	}
	private Vector3 slowdownV;
	private Vector2 horizontalMovement;
	
	void FixedUpdate(){
		PlayerMovementLogic ();
	}
	/*
	* Accordingly to input adds force and if magnitude is bigger it will clamp it.
	* If player leaves keys it will deaccelerate
	*/
	void PlayerMovementLogic(){
		currentSpeed = rb.velocity.magnitude;
		horizontalMovement = new Vector2 (rb.velocity.x, rb.velocity.z);
		if (horizontalMovement.magnitude > maxSpeed){
			horizontalMovement = horizontalMovement.normalized;
			horizontalMovement *= maxSpeed;    
		}
		rb.velocity = new Vector3 (
			horizontalMovement.x,
			rb.velocity.y,
			horizontalMovement.y
		);
		if (grounded){
			rb.velocity = Vector3.SmoothDamp(rb.velocity,
				new Vector3(0,rb.velocity.y,0),
				ref slowdownV,
				deaccelerationSpeed);
		}

		if (grounded) {
			rb.AddRelativeForce (Input.GetAxis ("Horizontal") * accelerationSpeed * Time.deltaTime, 0, Input.GetAxis ("Vertical") * accelerationSpeed * Time.deltaTime);
		} else {
			rb.AddRelativeForce (Input.GetAxis ("Horizontal") * accelerationSpeed / 2 * Time.deltaTime, 0, Input.GetAxis ("Vertical") * accelerationSpeed / 2 * Time.deltaTime);

		}
		/*
		 * Slippery issues fixed here
		 */
		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			deaccelerationSpeed = 0.5f;
		} else {
			deaccelerationSpeed = 0.1f;
		}
	}
	/*
	* Handles jumping and ads the force and sounds.
	*/
	void Jumping(){
		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			rb.AddRelativeForce (Vector3.up * jumpForce);
			if (_jumpSound) { 
				source.Stop();
				source.PlayOneShot(_jumpSound);
			}
			else
				print ("Missig jump sound.");
			
		}
	}
	/*
	* Update loop calling other stuff
	*/
	void Update(){
		

		Jumping ();

		Crouching();

		WalkingSound ();


	}//end update

	/*
	* Checks if player is grounded and plays the sound accorindlgy to his speed
	*/
	void WalkingSound(){
		if (_walkSound && _runSound) {
			if (RayCastGrounded ()) { //for walk sounsd using this because suraface is not straigh			
				if (currentSpeed > 1) {
					if (maxSpeed == 3) {
						if (source.isPlaying) {
							source.Stop ();
							source.PlayOneShot (_walkSound);
						}					
					} else if (maxSpeed == 5) {
						//	print ("NE tu sem");

						if (!source.isPlaying) {
							source.Stop ();
							source.PlayOneShot (_runSound);
						}
					}
				} else {source.Stop ();
				}
			} else {
				source.Stop ();
			}
		} else {
			print ("Missing walk and running sounds.");
		}

	}
	/*
	* Raycasts down to check if we are grounded along the gorunded method() because if the
	* floor is curvy it will go ON/OFF constatly this assures us if we are really grounded
	*/
	private bool RayCastGrounded(){
		RaycastHit groundedInfo;
		if(Physics.Raycast(transform.position, transform.up *-1f, out groundedInfo, 1, ~ignoreLayer)){
			Debug.DrawRay (transform.position, transform.up * -1f, Color.red, 0.0f);
			if(groundedInfo.transform != null){
				//print ("vracam true");
				return true;
			}
			else{
				//print ("vracam false");
				return false;
			}
		}
		//print ("nisam if dosao");

		return false;
	}

	/*
	* If player toggle the crouch it will scale the player to appear that is crouching
	*/
	void Crouching(){
		if(Input.GetKey(KeyCode.C)){
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,0.6f,1), Time.deltaTime * 15);
		}
		else{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), Time.deltaTime * 15);

		}
	}


	[Tooltip("The maximum speed you want to achieve")]
	public int maxSpeed = 5;

	[Tooltip("The higher the number the faster it will stop")]
	public float deaccelerationSpeed = 15.0f;

	[Tooltip("Force that is applied when moving forward or backward")]
	public float accelerationSpeed = 50000.0f;

	[Tooltip("Tells us weather the player is grounded or not.")]
	public bool grounded;
	/*
	* checks if our player is contacting the ground in the angle less than 60 degrees
	*	if it is, set groudede to true
	*/
	void OnCollisionStay(Collision other){
		foreach(ContactPoint contact in other.contacts){
			if(Vector2.Angle(contact.normal,Vector3.up) < 60){
				grounded = true;
			}
		}
	}
	/*
	* On collision exit set grounded to false
	*/
	void OnCollisionExit ()
	{
		grounded = false;
        if (source.isPlaying)
        {
			source.Stop();
        }
		source.PlayOneShot(_groundedSound);
	}

	[Tooltip("Put 'Player' layer here")]
	[Header("Shooting Properties")]
	private LayerMask ignoreLayer;//to ignore player layer
	[Tooltip("Put BulletSpawn gameobject here, palce from where bullets are created.")]
	[HideInInspector]
	public Transform bulletSpawn; //from here we shoot a ray to check where we hit him;

	[Header("Player SOUNDS")]
	[Tooltip("Jump sound when player jumps.")]
	public AudioClip _jumpSound;
	[Tooltip("Walk sound player makes.")]
	public AudioClip _walkSound;
	[Tooltip("Run Sound player makes.")]
	public AudioClip _runSound;
    public  AudioClip _groundedSound;
}

