using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject FadeOut;
    public void Play()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayFadeIn()
    {
        FadeOut.SetActive(true);
        Invoke("Play", 3f);

    }
}
