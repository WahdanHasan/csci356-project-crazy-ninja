using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorAudio : MonoBehaviour
{
    [SerializeField] AudioClip narrator_audio;

    bool audio_played = false;
    private void OnTriggerEnter(Collider other)
    {
        if (audio_played) return;

        if(other.tag == "Player")
        {
            audio_played = true;

            GetComponent<AudioSource>().PlayOneShot(narrator_audio);
        }
    }
}
