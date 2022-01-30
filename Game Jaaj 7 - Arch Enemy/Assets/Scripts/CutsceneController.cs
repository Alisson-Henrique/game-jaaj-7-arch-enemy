using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject FadeOut;

    public void PlayFadeOut()
    {
        FadeOut.SetActive(true);
    }
}
