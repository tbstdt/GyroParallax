using System;
using UnityEngine;

public class gyroController : MonoBehaviour {

		private bool isGyroEnabled;
	private Gyroscope gyro;
	private Quaternion startRot;
	private Quaternion rotSaver;

	[Range(0,1)]
	public float speed = 0.1f;
	[Range(0,1)]
	public float touchSpeed = 0.02f;


	private float xPos;
	private float yPos;

	public float maxR = 4f;
	public float maxU = 2f;
	public float minD = -2f;
	public float minL = -4f;

	void Start ()
	{
		isGyroEnabled = EnableGyro();
		startRot = transform.rotation;
	}


	private void FixedUpdate()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) 
		{
			TouchMover();
		}
		else if (isGyroEnabled) GyroMover();
	}

	private void DoRotate(float x, float y)
	{
	    xPos = Mathf.Clamp(x, minL, maxR);
		yPos = Mathf.Clamp(y, minD, maxU);		

		var Y = Quaternion.AngleAxis(-xPos, Vector3.up);
		var X = Quaternion.AngleAxis(yPos, Vector3.right);

		rotSaver = startRot * Y * X;
		transform.rotation = rotSaver;
	}


	private void TouchMover()
	{		
		isGyroEnabled = false;
		xPos += Input.GetTouch(0).deltaPosition.x * touchSpeed;
		yPos += Input.GetTouch(0).deltaPosition.y * touchSpeed;	

		DoRotate(x,y);
	}

	private void GyroMover()
	{
		xPos += Input.gyro.rotationRateUnbiased.y * speed;
		yPos += Input.gyro.rotationRateUnbiased.x * speed;

		DoRotate(x,y);
	}

	private bool EnableGyro()
	{
		if (SystemInfo.supportsGyroscope)
		{
			gyro = Input.gyro;
			gyro.enabled = true;
			return true;
		}
		return false;
	}

	
	
	private void OnGUI()
	{
		var style = new GUIStyle();
		style.fontSize = 36;
		style.normal.textColor = Color.white;
		GUI.Label(new Rect(10, 10, 200, 50),  Input.acceleration.ToString(), style);
	}
}