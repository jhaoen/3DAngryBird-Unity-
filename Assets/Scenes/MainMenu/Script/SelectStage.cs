using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    public GameObject LevelButtonPrefab;
	public GameObject LockLevelButtonPrefab;
    public List<SceneAsset> stageName = new List<SceneAsset>();
    public Vector3 StartPosition = new Vector3(-80, 45, 0);
    public float LevelDistanceGap = 10f;
    public int UnlockLevel = 5;
    public int row = 5;
    public int colum = 5;

	// Start is called before the first frame update
	void Start()
    {
        CreateLevel(row, colum);

	}

    void CreateLevel(int row,int colum)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < colum; j++)
            {
				GameObject newButton;
                int nowLevel = i * colum + j;
				if (nowLevel<=UnlockLevel)
                {
                    newButton = Instantiate(LevelButtonPrefab, transform);
				}
                else
                {
                    newButton = Instantiate(LockLevelButtonPrefab,transform);
                }
                TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                    buttonText.text = nowLevel.ToString();
                newButton.transform.localPosition = StartPosition + new Vector3(j * LevelDistanceGap, -i * LevelDistanceGap, 0);
                Button buttonComponent = newButton.GetComponent<Button>();
                if(stageName.Count > nowLevel)
                    buttonComponent.onClick.AddListener(()=> GoToScene(nowLevel,stageName[nowLevel].name));
            }
        }
    }

    public void GoToScene(int level,string sceneName)
    {
        PlayerPrefs.SetInt("Level",level);
        SceneManager.LoadScene(sceneName);
    }
}
