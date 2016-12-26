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

	// Use this for initialization
	void Start () {
		horizontalAim = 0f;
		verticalAim = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	void updatePositions () {
		// convert aim values into global position
		horizontalPosition = horizontalAim / 100f * (MAX_HORIZ - MIN_HORIZ) + MIN_HORIZ;
		verticalPosition = verticalAim / 100f * (MAX_VERT - MIN_VERT) + MIN_VERT;

		horizontal_aim_bar.position = new Vector3(horizontal_aim_bar.position.x, horizontalPosition, horizontal_aim_bar.position.z);
		vertical_aim_bar.position = new Vector3(vertical_aim_bar.position.x, verticalPosition, vertical_aim_bar.position.z);
	}
}
