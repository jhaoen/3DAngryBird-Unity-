using System;
using UnityEngine;

public class RushBird : NormalBird
{
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		planeBounds = GameManager.planeBounds;

	}


	protected override void CustomUpdate()
	{
		base.CustomUpdate();
		if(Input.GetKeyDown(KeyCode.Space) && isShoot)
		{
			//Quaternion cameraDirection = GameManager.Instance.GetCameraDirection();
			//Debug.Log(cameraDirection.eulerAngles);
			//transform.rotation = cameraDirection;
			rb.velocity = transform.forward * 40.0f;
		}
	}


}
