using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject Menu;
	public GameObject ResultMenu;
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI HUDScoreText;
	public TextMeshProUGUI ResultText;
	public Button NextLevelButton;

	public SceneAsset mainMenu;

	private void Start()
	{
		Debug.Assert(Menu != null, "need set Menu on GameMenu");
		Debug.Assert(ResultMenu != null, "need set ResultMenu on GameMenu");
		Debug.Assert(levelText != null, "need set levelText on GameMenu");


		levelText.text = PlayerPrefs.GetInt("Level", 0).ToString();
		Menu.SetActive(false);
		ResultMenu.SetActive(false); ;

	}
	public void OpenMenu()
    {
        Menu.SetActive(true);
		Time.timeScale = 0f; 
	}
	public void CloseMenu()
	{
		Menu.SetActive(false);
		Time.timeScale = 1f; 
	}
	public void BackMainMenu()
	{
		Time.timeScale = 1f;	
		SceneManager.LoadScene(mainMenu.name);
	}
	public void ReloadGame()
	{
		int currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
		Time.timeScale = 1f;
		SceneManager.LoadScene(currentSceneIdx); 
	}
	public void OpenResultMenu(bool isWin)
	{
		Time.timeScale = 1f;
		Menu.SetActive(false);
		ResultMenu.SetActive(true);
		if (isWin)
			ResultText.text = "Win!!";
		else
			ResultText.text = "Good Try";
		NextLevelButton.enabled = isWin;
	}
	public void NextLevel()
	{
		Time.timeScale = 1f;
		int currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIdx+1); // defalut set next idx is next level
	}
	public void SetScoreText(string score)
	{
		scoreText.text = score;
		HUDScoreText.text = score;
	}
}
