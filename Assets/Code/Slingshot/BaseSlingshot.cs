using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BaseSlingshot : MonoBehaviour
{
	[HideInInspector]
	public LineRenderer lineRenderer;
	private Animator animator;
	private Transform LeftPad, RightPad;
	private bool showAimLine=false;
	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		LeftPad = transform.Find("SlingshotArmature/handle/wood4.l/wood3.l/wood2.l/sling3.l/sling2.l/sling1.l/sling1.l_end");
		RightPad = transform.Find("SlingshotArmature/handle/wood4.r/wood3.r/wood2.r/sling3.r/sling2.r/sling1.r/sling1.r_end");
	}
	private void Update()
	{
		if (showAimLine)
		{
			lineRenderer.positionCount = 2;
			Vector3 startPosition = transform.position+new Vector3(0,0.3f,0)*transform.localScale.y;
			lineRenderer.SetPosition(1, startPosition);
			Vector3 aimDirection = (startPosition - GetPadPosition()).normalized;
			Vector3 endPoint = startPosition + aimDirection * 20f;

			lineRenderer.SetPosition(0, endPoint); 
		}
		else
		{
			lineRenderer.positionCount = 0;
		}
	}

	public void Charging()
	{
		showAimLine = true;
		animator.SetBool("isShooting", false);
		animator.SetBool("isCharging",true);
	}
	public void Release()
	{
		showAimLine = false;
		animator.SetBool("isCharging", false);
		animator.SetBool("isShooting", true);
	}
	public float GetAnimationTime()
	{
		return animator.GetCurrentAnimatorClipInfo(0).Length;
	}
	public Vector3 GetPadPosition()
	{
		return (LeftPad.position + RightPad.position)/2;
	}
}
