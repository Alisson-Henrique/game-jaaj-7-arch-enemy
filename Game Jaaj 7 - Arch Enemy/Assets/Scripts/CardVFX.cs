using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVFX : MonoBehaviour
{
    public string vfxName;

    public void OnParticleSystemStopped()
    {
        Debug.Log("Stop " + vfxName);
    }
}
