using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject FadeOut;

    private AudioSource audioSource;

    private GameController gameController;

    public GameObject SkipButton;

    public GameObject panelDead;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gameController.isDead = false;
            gameController.NextScene("Gameplay");
        }
    }

    public void PlayFadeOut()
    {
        FadeOut.SetActive(true);
    }

    public void SkipCutscene()
    {
        gameController.skipCutscene();
    }

    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        audioSource = GetComponent<AudioSource>();

        SkipButton.SetActive(true);

        if (gameController.isDead)
        {
            panelDead.SetActive(true);
            SkipButton.SetActive(false);
        }

        audioSource.clip = gameController.GetCutsceneAudioClip();
        audioSource.Play();
    }
}
