using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mainControl : MonoBehaviour
{
    public Transform slingShot;
	public Transform dummyRotateObject;

	public List<Transform> BirdList = new List<Transform>();
    public GameObject Camera;
    public Vector3 slingPadOffset = new Vector3(0, 0, 2f);
    public float rotationSpeed = 0.1f;


	private Queue<Transform> ShootOrder = new Queue<Transform>();
    private Transform NextBird;
	private NormalBird NextBirdControl;
    private cameraControl MainCameraControl;
    private BaseSlingshot baseSlingshot;
	private Vector3 previousMousePosition;


    private bool isBirdOnSlingShot = false;
    private bool isDragging = false;
    private bool isBirdShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in BirdList)
            ShootOrder.Enqueue(t);
        bool haveNext = ShootOrder.TryDequeue(out NextBird);
        if(haveNext)
		    NextBirdControl = NextBird.GetComponent<NormalBird>();
		MainCameraControl = Camera.GetComponent<cameraControl>();
		baseSlingshot = slingShot.GetComponent<BaseSlingshot>();

    }

    // Update is called once per frame
    void Update()
    {
		if (isBirdOnSlingShot)
			NextBirdControl.SetHoldPosition(baseSlingshot.GetPadPosition() + slingPadOffset);
		else if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PrepareSlingshot(NextBirdControl.launchTime));
        }

        if (isBirdOnSlingShot)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isDragging = true;
                previousMousePosition = Input.mousePosition;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isDragging = false;
                if (!isBirdShooting)
                {
                    isBirdShooting = true;
                    baseSlingshot.Release();
                    StartCoroutine(ShootSlingshot(baseSlingshot.GetAnimationTime()));
                }
            }
            if (isDragging)
            {
                Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;

                float horizontalRotation = -deltaMousePosition.x * rotationSpeed;
                float verticalRotation = deltaMousePosition.y * rotationSpeed;
				

				previousMousePosition = Input.mousePosition;
				if (!isBirdShooting)
				{
					slingShot.Rotate(Vector3.up, horizontalRotation, Space.World);
					slingShot.Rotate(Vector3.right, verticalRotation, Space.Self);
					NextBirdControl.SetBirdDirection(slingShot.rotation);
				}
				else
				{
					Quaternion simulatedRotation = Quaternion.identity;
					simulatedRotation *= Quaternion.Euler(verticalRotation, horizontalRotation, 0); // �ϥβֿn����
					MainCameraControl.UpdateAngles(simulatedRotation);
				}
				
            }
        }
        

	}


	IEnumerator PrepareSlingshot(float delay)
    {
		isBirdOnSlingShot = true;
		NextBirdControl.StartLoadingSlingShot(baseSlingshot.GetPadPosition() + slingPadOffset);
		yield return new WaitForSeconds(delay);
		baseSlingshot.Charging();
	}

	IEnumerator ShootSlingshot(float delay)
    {
		yield return new WaitForSeconds(delay);
		NextBirdControl.SetBirdDirection(slingShot.rotation);
		NextBirdControl.StartShoot();
		MainCameraControl.SetTempFollow(NextBird.transform, 5);
		//NormalBird PrevBirdControl = NextBirdControl;
        yield return new WaitForSeconds(5.5f);
		slingShot.rotation = Quaternion.identity;
		bool haveNext = ShootOrder.TryDequeue(out NextBird);
		if (haveNext)
        {
			NextBirdControl = NextBird.GetComponent<NormalBird>();
			isBirdOnSlingShot = false;
            isBirdShooting = false;
		}
        else if(!GameManager.Instance.isWin)
        {
            while (!GameManager.Instance.AreAllObjectsStopped())
            {
                yield return new WaitForSeconds(0.5f);
            }
            GameManager.Instance.GameLoss();
        }
	}

    public ref Queue<Transform> GetShootOrder()
    {
        return ref ShootOrder;
    }
    
}
