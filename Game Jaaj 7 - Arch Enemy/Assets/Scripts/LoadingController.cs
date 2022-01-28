using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Invoke("nextScene", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void nextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
