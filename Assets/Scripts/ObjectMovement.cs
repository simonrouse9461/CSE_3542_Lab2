using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectMovement : MonoBehaviour
{
	public Transform cameraTransform;
	public Vector3 startingPoint;
	Rigidbody body;

	bool _applyPhysics;
	bool applyPhysics
	{
		get
		{
			return _applyPhysics;
		}
		set
		{
			_applyPhysics = value;
			body.useGravity = value;
			body.isKinematic = !value;
			body.detectCollisions = value;
		}
	}

	void TogglePhysics(bool? value = null)
	{
		applyPhysics = value ?? !applyPhysics;
	}

	void Reset()
	{
		transform.position = startingPoint;
		transform.rotation = new Quaternion();
		transform.localScale = new Vector3(1, 1, 1);
		body.velocity = new Vector3();
		body.angularVelocity = new Vector3();
	}

	// Use this for initialization
	void Start()
	{
		body = GetComponent<Rigidbody>();
		TogglePhysics(true);
		Reset();
	}

	// Update is called once per frame
	void Update()
	{
		var linear = new Vector3();
		var angular = new Vector3();
		float speed = 0.1f, omega = 2, force = 15, torque = 20, scale = 0.05f;

		var linearMapping = new Dictionary<KeyCode, Vector3>
		{
			{KeyCode.LeftShift, Vector3.up},
			{KeyCode.LeftControl, Vector3.down},
			{KeyCode.A, Vector3.left},
			{KeyCode.D, Vector3.right},
			{KeyCode.W, Vector3.forward},
			{KeyCode.S, Vector3.back}
		};

		var angularMapping = new Dictionary<KeyCode, Vector3>
		{
			{KeyCode.UpArrow, Vector3.right},
			{KeyCode.DownArrow, Vector3.left},
			{KeyCode.LeftArrow, Vector3.forward},
			{KeyCode.RightArrow, Vector3.back},
			{KeyCode.Comma, Vector3.up},
			{KeyCode.Period, Vector3.down}
		};

		foreach (var pair in linearMapping)
		{
			if (Input.GetKey(pair.Key))
			{
				linear += pair.Value;
			}
		}

		foreach (var pair in angularMapping)
		{
			if (Input.GetKey(pair.Key))
			{
				angular += pair.Value;
			}
		}

		if (Input.GetKey(KeyCode.Equals))
		{
			transform.localScale *= 1 + scale;
		}
		if (Input.GetKey(KeyCode.Minus))
		{
			transform.localScale *= 1 - scale;
		}

		// reset
		if (Input.GetKey(KeyCode.R))
		{
			Reset();
		}

		// toggle physics
		if (Input.GetKeyDown(KeyCode.P))
		{
			TogglePhysics();
		}

		var fixingRotation = cameraTransform.rotation;
		fixingRotation.x = 0;
		var fixedLinear = fixingRotation * linear;
		var fixedAngular = fixingRotation * angular;

		if (applyPhysics)
		{
			// apply force and torque
			body.AddForce(fixedLinear * force);
			body.AddTorque(fixedAngular * torque);
		}
		else
		{
			// apply movement and rotation
			transform.Translate(fixedLinear * speed, Space.World);
			transform.Rotate(fixedAngular * omega, Space.World);
		}
	}

	void FixedUpdate()
	{

	}
}
