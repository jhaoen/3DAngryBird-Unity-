using System;
using UnityEngine;

public class NormalBird : MonoBehaviour
{
    public float launchTime = 1f;   // init speed
	public GameObject explosionEffect;

    protected Rigidbody rb;
	protected bool isLoadingSlingShot = false;
	protected bool isPrepareToShoot = false;
	protected bool isShoot = false;


	protected Vector3 startPosition;
	protected Vector3 endPosition;
	protected float timeElapsed = 0f;  // move time

	protected bool hasCollided = false; 
	protected bool isStopped = false;   
	protected bool isLaunched = false;  
	protected Bounds planeBounds;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		planeBounds = GameManager.planeBounds;

	}

	void Update()
    {
		CustomUpdate();
	}
	protected virtual void CustomUpdate()
	{
		if (isShoot && rb.velocity.magnitude > 15f)
			transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);

		if (isLoadingSlingShot)
		{
			LoadingSlingShot();
		}
		else if (isPrepareToShoot)
		{
			FollowEndPosition();
		}
		if (isLaunched && hasCollided && !isStopped && rb.velocity.magnitude == 0f)
		{
			OnBirdStopped();
		}

		CheckOutOfBounds();
	}

	public void SetHoldPosition(Vector3 holdPosition)
	{
		endPosition = holdPosition;
	}

    public void StartLoadingSlingShot(Vector3 holdPostion)
    {
		startPosition = transform.position;
		gameObject.transform.rotation = Quaternion.identity;
		endPosition = holdPostion ;
		timeElapsed = 0f;
		isLoadingSlingShot = true;
	}

    protected void LoadingSlingShot()
    {
		timeElapsed += Time.deltaTime;

		float normalizedTime = timeElapsed / launchTime;

		if (normalizedTime > 1f)
		{
			LoadingSlingShotDone();
			return;
		}

		Vector3 horizontalPosition = Vector3.Lerp(startPosition, endPosition, normalizedTime);

		transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y, horizontalPosition.z);
	}


	protected void LoadingSlingShotDone()
	{
		gameObject.transform.rotation = Quaternion.identity;
		isLoadingSlingShot = false;
		transform.position = endPosition; 
		isPrepareToShoot = true;
	}

	public void FollowEndPosition()
	{
		transform.position = new Vector3(endPosition.x, endPosition.y, endPosition.z);
	}
	protected void OnCollisionEnter(Collision collision)
	{
		if (!isLaunched) return; 

		hasCollided = true; 
	}

	public void OnBirdStopped()
	{
		isStopped = true; // �аO���w�R��

		// �q�� mainControl �p�����
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnBirdActionComplete();
		}

		if (explosionEffect != null)
		{
			GameObject explode = Instantiate(explosionEffect, transform.position, Quaternion.identity);
			Destroy(explode,3f);
		}

		if(gameObject != null)
        {
			Destroy(gameObject); 
		}
	}

	protected void CheckOutOfBounds()
	{
		if (transform.position.x < planeBounds.min.x || transform.position.x > planeBounds.max.x ||
		transform.position.z < planeBounds.min.z || transform.position.z > planeBounds.max.z)
		{
			Debug.Log($"Plane Bound at {planeBounds}");
			OnBirdStopped();
		}
	}
	public void SetBirdDirection(Quaternion targetRotation)
	{
		transform.rotation = targetRotation;
	}

	public void StartShoot()
    {
		isPrepareToShoot = false;
		isShoot = true;
		rb.velocity = transform.forward * 40.0f;
	}

	


}
