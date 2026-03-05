using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
	public Transform defaultTarget;
	public Transform tempTarget;

	public Vector3 offset = new Vector3(0, 20, -60);
	public Vector3 tempOffset = new Vector3(0, 5, -15);
	public float smoothSpeed = 0.125f;
	public Vector3 initVelocity = Vector3.zero;
	public float radius = 10f;
	public float xAngel = 30;
	public float yAngel = 90;

	private Vector3 defaultOffset;
	private Vector3 prevRotationPosition;
	private float remainFollowTime;
	public float currentXAngle;
	public float currentYAngle;

	public float manualRotateSpeed = 90f;
	public float RotateRadius = 200f;

	private void Start()
	{
		defaultOffset = offset;
		ResetCameraPosition();
		
	}
	void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ResetCameraPosition();
		}

		if (tempTarget!=null || defaultTarget != null)
		{
			Transform followTarget = defaultTarget;
			if (tempTarget != null)
			{
				followTarget = tempTarget;
				remainFollowTime -= Time.deltaTime;
				if (remainFollowTime < 0)
				{
					tempTarget = null;
					offset = defaultOffset;
					followTarget = defaultTarget;
				}
			}
			else
			{
				//prevRotationPosition = ComputeSlingshotPosition(followTarget);

				GameObject center = GameObject.Find("Center");
				if(center == null)
                {
					Debug.Log("Center object not found");
                }
				followTarget = center.transform;
				if (Input.GetKey(KeyCode.A)) // left rotate
				{
					
					currentYAngle -= manualRotateSpeed * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.S)) // right rotate
				{
					currentYAngle += manualRotateSpeed * Time.deltaTime;
				}

				float xDistance = Mathf.Cos(currentYAngle * Mathf.PI / 180) * RotateRadius;
				float zDistance = Mathf.Sin(currentYAngle * Mathf.PI / 180) * RotateRadius;
				float yDistance = Mathf.Sin(currentXAngle * Mathf.PI / 180) * zDistance;
				zDistance = Mathf.Cos(currentXAngle * Mathf.PI / 180) * zDistance;
				prevRotationPosition = new Vector3(xDistance, yDistance, zDistance);
				Debug.Log($"new camera position : {prevRotationPosition}");
			}
			Vector3 desiredPosition = prevRotationPosition + followTarget.position;

			transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref initVelocity, 0.1f);

			transform.LookAt(followTarget);
		}
	}

	public void UpdateAngles(Quaternion newAngle)
	{
		currentXAngle += newAngle.eulerAngles.x;
		currentYAngle += newAngle.eulerAngles.y;

		float xDistance = Mathf.Cos(currentYAngle * Mathf.PI / 180) * radius;
		float zDistance = Mathf.Sin(currentYAngle * Mathf.PI / 180) * radius;
		float yDistance = Mathf.Sin(currentXAngle * Mathf.PI / 180) * zDistance;
		zDistance = Mathf.Cos(currentXAngle * Mathf.PI / 180) * zDistance;
		prevRotationPosition = new Vector3(xDistance, yDistance, zDistance);
	}

	Vector3 ComputeSlingshotPosition(Transform followTarget)
	{
		currentXAngle = -(followTarget.eulerAngles.x + xAngel);
		currentYAngle = -(followTarget.eulerAngles.y + yAngel);
		// restrict angel
		if (currentXAngle >= -20)
			currentXAngle = -20;
		else if (currentXAngle >= -360 && currentXAngle < -35)
		{
			if (Mathf.Abs(currentXAngle - (-360)) > Mathf.Abs(currentXAngle - (-35)))
				currentXAngle = -40;
			else
				currentXAngle = -360;
		}

		if (currentYAngle >= -90)
			currentYAngle = -90;
		else if (currentYAngle >= -360 && currentYAngle < -180)
		{
			if (Mathf.Abs(currentYAngle - (-360)) > Mathf.Abs(currentYAngle - (-180)))
				currentYAngle = -180;
			else
				currentYAngle = -360;
		}

		float xDistance = Mathf.Cos(currentYAngle * Mathf.PI / 180) * radius;
		float zDistance = Mathf.Sin(currentYAngle * Mathf.PI / 180) * radius;
		float yDistance = Mathf.Sin(currentXAngle * Mathf.PI / 180) * zDistance;
		zDistance = Mathf.Cos(currentXAngle * Mathf.PI / 180) * zDistance;
		return new Vector3(xDistance, yDistance, zDistance);
	}

	public void SetTempFollow(Transform followTarget, float followTime)
	{
		remainFollowTime = followTime;
		tempTarget = followTarget;
		offset = tempOffset;
	}
	public Quaternion GetCameraDirection()
	{
		return Quaternion.Euler(-currentXAngle - xAngel, -currentYAngle - yAngel, 0);
	}
	private float NormalizeAngle(float angle)
	{
		while (angle > 180) angle -= 360;
		while (angle < -180) angle += 360;
		return angle;
	}

	public void ResetCameraPosition()
	{
		currentYAngle = -90f;
		currentXAngle = 0f;

		float xDistance = Mathf.Cos(currentYAngle * Mathf.PI / 180) * RotateRadius;
		float zDistance = Mathf.Sin(currentYAngle * Mathf.PI / 180) * RotateRadius;
		float yDistance = Mathf.Sin(currentXAngle * Mathf.PI / 180) * zDistance;
		zDistance = Mathf.Cos(currentXAngle * Mathf.PI / 180) * zDistance;

		prevRotationPosition = new Vector3(xDistance, yDistance, zDistance);

		transform.position = prevRotationPosition + defaultTarget.position;
		transform.LookAt(defaultTarget);
	}
}
