using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballPitch : MonoBehaviour {

	public Transform horizontal_aim_bar;
	public Transform vertical_aim_bar;
	private float horizontalPosition;
	private float verticalPosition;
	private float horizontalAim;
	private float verticalAim;
	private float MAX_VERT = 1.06f;
	private float MIN_VERT = 0.46f;
	private float MAX_HORIZ = 0.2159f;
	private float MIN_HORIZ = -0.2159f;
	private bool flagThrown = false;
	private float pitchVert;
	private float pitchHoriz;
	private float pitchVertPosition, pitchHorizPosition;
	private Rigidbody rbBaseball;
	private float vertError, horizError, horizErrorPast, vertErrorPast;
	private float horizIntegrator, vertIntegrator, horizDerivative, vertDerivative;
	private float Kp, Ki, Kd, Kp_default, Ki_default, Kd_default;
	private bool flagNearGains = false;
	private float pitchTime;

	// Use this for initialization
	void Start () {
		horizontalAim = 0f;
		verticalAim = 0f;
		rbBaseball = GetComponent<Rigidbody> ();
		Kp_default = 0.04f;
		Kd_default = 0.02f;
		Ki_default = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

		//Dummy code for new pitch target with button press
		if (Input.GetButtonDown ("Jump")) {
			pitch ();
			pitchTime = Time.time;
			aimGuess ();
			updateAimPositions ();
		}

		if (Time.time - pitchTime >= Random.Range(0.43f, 0.6f)) {
			// Pitch has reached plate by this time
			flagThrown = false;
		}

		
	}

	void FixedUpdate () {
		if (flagThrown == true) {
			aimPIDUpdate ();
			updateAimPositions ();
		}

	}

	void updateAimPositions () {
		// convert aim values into global position
		horizontalPosition = horizontalAim / 100f * (MAX_HORIZ - MIN_HORIZ) + MIN_HORIZ;
		verticalPosition = verticalAim / 100f * (MAX_VERT - MIN_VERT) + MIN_VERT;

		horizontal_aim_bar.position = new Vector3(horizontal_aim_bar.position.x, horizontal_aim_bar.position.y, horizontalPosition);
		vertical_aim_bar.position = new Vector3(vertical_aim_bar.position.x, verticalPosition, vertical_aim_bar.position.z);
	}


	void pitch () {
		pitchVert = Random.Range (0f, 100f);
		pitchVertPosition = pitchVert / 100f * (MAX_VERT - MIN_VERT) + MIN_VERT;
		pitchHoriz = Random.Range (0f, 100f);
		pitchHorizPosition = pitchHoriz / 100f * (MAX_HORIZ - MIN_HORIZ) + MIN_HORIZ;
		//Update pitch projected location
		rbBaseball.transform.position = new Vector3(0f, pitchVertPosition, pitchHorizPosition);
		flagThrown = true;
	}

	void aimGuess () {
		horizontalAim = 50f;
		verticalAim = 50f;
		horizIntegrator = 0f;
		vertIntegrator = 0f;

		// Based on aim guess, use near or far gains
		if (Mathf.Abs (pitchHoriz - horizontalAim) > 20f || Mathf.Abs (pitchVert - verticalAim) > 20f) {
			// Far gains
			Kp = 0.05f;
			Kd = 0.2f;
			Ki = 0.2f;
		} else {
			// Near gains
			Kp = 0.2f;
			Kd = 0.4f;
			Ki = 0.01f;
		}
	
	}

	void aimPIDUpdate(){
		// Update aim position using PID control
		horizError = pitchHoriz - horizontalAim;
		vertError = pitchVert - verticalAim;
		horizIntegrator = Mathf.Min(10f, Mathf.Max(-10f, horizIntegrator + horizError));
		vertIntegrator = Mathf.Min(10f, Mathf.Max(-10f, vertIntegrator + vertError));
		horizDerivative = Mathf.Min (5f, Mathf.Max (-5f, horizError - horizErrorPast));
		vertDerivative = Mathf.Min (5f, Mathf.Max (-5f, vertError - vertErrorPast));

//		if ( Mathf.Abs( horizError) > 10f || Mathf.Abs( vertError) > 10f && !flagNearGains) {
//			Kp = 1f * Kp_default;
//			Kd = Kd_default;
//			Ki = 0.3f; //* Ki_default;
//		}
//		else {
//			Kp = Kp_default;
//			Kd = Kd_default;
//			Ki = Ki_default;
//			flagNearGains = true;
//			print (flagNearGains);
//		}

		horizontalAim = horizontalAim + Kp * horizError + Ki * horizIntegrator + Kd * horizDerivative;
		verticalAim = verticalAim + Kp * vertError + Ki * vertIntegrator + Kd * vertDerivative;
		horizontalAim = Mathf.Min (100f, Mathf.Max (0f, horizontalAim));
		verticalAim = Mathf.Min (100f, Mathf.Max (0f, verticalAim));

		horizErrorPast = horizError;
		vertErrorPast = vertError;

	}
}
