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
		if (isGyroEnabled)
		{
			var rotate = Input.gyro.rotationRateUnbiased;
			rotate.z = 0;
			RotateAndShift(rotate);

			var angle = Quaternion.Angle( startRotation, Rotator.transform.rotation );
			if (angle > MaxAnglie)
				RotateAndShift(-rotate);
		}
	}

	private void RotateAndShift(Vector3 rotate)
	{
		Rotator.transform.Rotate(rotate * Accuracy);
		Rotator.transform.position += new Vector3(rotate.y, rotate.x) * Shift;
	}
}