using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Dialog[] cutcenesDialogs;

    public GameObject[] EnemyPrefabs;

    public GameObject[] sceneryObejects;

    public Color[] colorScenerys;

    public int curretLevel;

    private int nextScene;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LevelUp()
    {
        curretLevel++;
        if(curretLevel == 3)
        {
            SceneManager.LoadScene("Credits");
        }
        else
        {
            SceneManager.LoadScene("Cutscene");
        }
    }

    public void Lost()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public GameObject GetScenery()
    {
        return sceneryObejects[curretLevel];
    }

    public Color GetColorScenery()
    {
        return colorScenerys[curretLevel];
    }
}
