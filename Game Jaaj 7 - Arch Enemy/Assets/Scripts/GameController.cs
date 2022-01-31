using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Dialog[] cutcenesDialogs;

    public GameObject[] EnemyPrefabs;

    public GameObject[] sceneryObejects;

    public AudioClip[] BattleAudioClips;
    public AudioClip[] CutsceneAudioClips;

    public Sprite[] ilustrationSprites;

    public Color[] colorScenerys;

    public int curretLevel;

    public bool isDead;

    private int nextScene;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
        isDead = true;
        SceneManager.LoadScene("Cutscene");
    }

    public GameObject GetScenery()
    {
        return sceneryObejects[curretLevel];
    }

    public Color GetColorScenery()
    {
        return colorScenerys[curretLevel];
    }

    public AudioClip GetCutsceneAudioClip()
    {
        if (isDead)
        {
            return CutsceneAudioClips[4];
        }


        return CutsceneAudioClips[curretLevel];
    }

    public Sprite GetIlustratio()
    {
        return ilustrationSprites[curretLevel];
    }

    public void skipCutscene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public AudioClip GetBattleAudioClip()
    {
        return BattleAudioClips[curretLevel];
    }

    public Sprite GetIlustrationDead()
    {
        return ilustrationSprites[3];
    }
}
