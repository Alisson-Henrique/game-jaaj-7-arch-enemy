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

    public Image ilustrationSprite;

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
        audioSource.clip = gameController.GetCutsceneAudioClip();
        ilustrationSprite.sprite = gameController.GetIlustratio();
        audioSource.Play();
    }
}
