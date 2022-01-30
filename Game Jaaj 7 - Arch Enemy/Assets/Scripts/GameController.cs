using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Dialog[] cutcenesDialogs;

    public GameObject[] EnemyPrefabs;

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

        SceneManager.LoadScene("Cutscene");

    }

    public void Lost()
    {

    }
}
