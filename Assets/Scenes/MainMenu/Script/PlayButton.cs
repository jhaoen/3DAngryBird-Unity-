using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Vector3 originalScale;
	void Start()
	{
		originalScale = transform.localScale;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		transform.localScale=(originalScale * 1.1f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		transform.localScale = originalScale;
	}
}
