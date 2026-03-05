using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaContorller:MonoBehaviour 
{
    public GameObject selectStageCanva;
	public GameObject mainMenuCanva;

	public void GoSelectStage()
    {
        selectStageCanva.SetActive(true);
        mainMenuCanva.SetActive(false);
    }

	public void GoMainMenu()
	{
		mainMenuCanva.SetActive(true);
		selectStageCanva.SetActive(false);
	}
}
