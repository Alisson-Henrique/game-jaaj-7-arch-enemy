using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    void Start()
    {
        Invoke("nextScene", 5f);
    }

    void nextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
