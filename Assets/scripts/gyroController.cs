using System;
using UnityEngine;

public class gyroController : MonoBehaviour {

	private bool isGyroEnabled;
	private Gyroscope gyro;
	private Quaternion startRotation;
	
	[Range(0,1)]
	public float Accuracy = 0.2f;
	[Range(5,20)]
	public float MaxAnglie = 10f;
	[Range(0,1)]
	public float Shift = 1f;
	
	public GameObject Rotator;
	
	void Start ()
	{
		isGyroEnabled = EnableGyro();
	}

	private bool EnableGyro()
	{
		if (SystemInfo.supportsGyroscope)
		{
			gyro = Input.gyro;
			gyro.enabled = true;
			startRotation = Rotator.transform.rotation;
			return true;
		}
		return false;
	}

	private void Update()
	{
		var rotate = Vector3.zero;
		if (!isGyroEnabled)
		{
			rotate = new Vector3(Input.gyro.rotationRateUnbiased.y,Input.gyro.rotationRateUnbiased.x, 0);
		}
		else
		{
			rotate = new Vector3(Input.acceleration.x, -Input.acceleration.y, 0);
		}
		
		RotateAndShift(rotate);
		CheckBounds(rotate);
	}
	
	
	private void RotateAndShift(Vector3 rotate)
	{
		Rotator.transform.Rotate(rotate * Accuracy);
		Rotator.transform.position += rotate * Shift;
	}

	private void CheckBounds(Vector3 rotate)
	{
		var angle = Quaternion.Angle(startRotation, Rotator.transform.rotation);
		if (angle > MaxAnglie)
			RotateAndShift(-rotate);
	}

	
	
	private void OnGUI()
	{
		var style = new GUIStyle();
		style.fontSize = 36;
		style.normal.textColor = Color.white;
		GUI.Label(new Rect(10, 10, 200, 50),  Input.acceleration.ToString(), style);
		GUI.Label(new Rect(10, 70, 200, 50),  Input.gyro.rotationRateUnbiased.ToString(), style);
	}
}