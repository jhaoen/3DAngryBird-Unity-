using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Bounds planeBounds;

    public bool isWin = false;
	public GameObject UI;

    private GameMenu gameMenu;
	private mainControl contorller;
    private int score = 0;
    private int PigRemaining = 0;

    void Awake()
    {
        // 確保只有一個 GameManager 實例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        GameObject plane = GameObject.Find("Plane");
        Terrain terrain = FindObjectOfType<Terrain>();

        if (plane != null)
        {
            
            Renderer planeRenderer = plane.GetComponent<Renderer>();
            if (planeRenderer != null)
            {
                planeBounds = planeRenderer.bounds;
                Debug.Log($"Plane Bounds:{planeBounds}");
            }
        }
        else if (terrain != null)
        {
            
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPosition = terrain.transform.position;
            Vector3 terrainSize = terrainData.size;

            planeBounds = new Bounds(
                terrainPosition + terrainSize / 2, 
                terrainSize                      
            );

            Debug.Log($"Plane Bounds:{planeBounds}");
        }
        else
        {
            Debug.LogError("No Plane or Terrain found in the scene.");
        }
        
        contorller = gameObject.GetComponent<mainControl>();
		gameMenu = UI.GetComponent<GameMenu>();
        isWin = false;
	}

	private void Start()
	{
        UpdateScoreUI();
	}

	public void RegisterPig()
    {

        PigRemaining++;
        Debug.Log($"豬數量增加：{PigRemaining}");
    }

    public void UnregisterPig()
    {
        // 擊倒一隻豬算 3000 分
        score += 3000;
        PigRemaining--;
        Debug.Log($"剩下：{PigRemaining} 隻豬");

        if (PigRemaining <= 0)
        {
            StartCoroutine(AllPigsDead());
        }
    }

    public int GetScore()
    {
        return score;
    }

    private IEnumerator AllPigsDead()
    {
        Debug.Log("所有豬都死亡了！");
        isWin = true;

		Queue<Transform> BirdQueue = contorller.GetShootOrder();
        
        Debug.Log($"剩餘 {BirdQueue.Count} 隻鳥");
        
        // 如果所有豬都被擊倒，並且場上還有鳥
        while (BirdQueue.Count > 0)
        {
            Transform bird = BirdQueue.Dequeue();
            
            if (bird == null)
            {
                Debug.LogWarning($"Bird is null or already destroyed.");
                continue;
            }

            NormalBird normalBird = bird.GetComponent<NormalBird>();
            if (normalBird != null)
            {
                normalBird.OnBirdStopped();
            }
            else
            {
                Debug.LogError($"Bird {bird.name} does not have a NormalBird script attached!");
            }

            yield return new WaitForSeconds(2f);
            score += 1000;

			UpdateScoreUI();
        }

        //while (!AreAllObjectsStopped())
        //{
        //    yield return new WaitForSeconds(0.5f);
        //}

        gameMenu.OpenResultMenu(isWin);

		Debug.Log($"Total Score : {score}");
        
    }

	public void UpdateScoreUI()
	{
		int score = GetScore();

		if (gameMenu != null)
		{
			gameMenu.SetScoreText(score.ToString());
		}
		else
		{
			Debug.Log($"No Score UI");
		}
	}

	public void OnBirdActionComplete()
	{
		UpdateScoreUI();
	}
    public void GameLoss()
    {
		gameMenu.OpenResultMenu(isWin);
	}

    public bool AreAllObjectsStopped()
    {
        
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            
            if (rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero)
            {
                return false;
            }
        }

        Transform[] transforms = FindObjectsOfType<Transform>();
        foreach (Transform obj in transforms)
        {
            if (obj.hasChanged) 
            {
                obj.hasChanged = false;
                return false;
            }
        }

        return true;
    }

    public Quaternion GetCameraDirection()
    {
        return contorller.Camera.GetComponent<cameraControl>().GetCameraDirection();
    }
}
