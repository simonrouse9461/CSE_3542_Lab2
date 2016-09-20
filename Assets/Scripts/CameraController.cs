using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	public Transform target;
	public Animator animator;
	public float xSpeed;
	public float ySpeed;
	public float rotateSpeed;
	public float rotateDistance;

	float xAngle;
	float yAngle;
	float _distance;
	float distance
	{
		get { return _distance; }
		set
		{
			_distance = value;
			if (_distance < 0)
			{
				_distance = 0;
			}
		}
	}
	Vector3 memory;
	bool rotate;

	void mouseMoveX(float value)
	{
		yAngle += value * xSpeed;
	}

	void mouseMoveY(float value)
	{
		xAngle -= value * ySpeed;
	}

	// Use this for initialization
	void Start () {
		xAngle = 30;
		yAngle = -30;
		distance = 10;
		rotate = false;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			if (rotate)
			{
				xAngle = memory.x;
				yAngle = memory.y;
				distance = memory.z;

				rotate = false;
			}
			else 
			{
				memory.x = xAngle;
				memory.y = yAngle;
				memory.z = distance;

				rotate = true;
				xAngle = 25;
				distance = rotateDistance;
			}
		}
		if (rotate)
		{
			yAngle += rotateSpeed;
		}
		else
		{
			if (Input.GetMouseButton(0))
			{
				mouseMoveX(Input.GetAxis("Mouse X"));
				mouseMoveY(Input.GetAxis("Mouse Y"));
			}
			distance -= Input.GetAxis("Mouse ScrollWheel") * 10;
		}
	}

	void LateUpdate()
	{
		Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0);
		Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;
		transform.rotation = rotation;
		transform.position = position;
	}
}