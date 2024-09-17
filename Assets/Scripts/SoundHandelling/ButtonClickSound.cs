using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    AudioSource ads;
    void Start()
    {
        ads = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        ads.Play();
    }
}
