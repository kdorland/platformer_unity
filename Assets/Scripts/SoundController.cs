using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    private AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip hitSound;
    public AudioClip killSound;
    public AudioClip coinSound;

    private void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    public void PlayPlayerHurtSound()
    {
        if (audioSource.enabled)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    public void PlayKillSound()
    {
        if (audioSource.enabled)
        {
            audioSource.PlayOneShot(killSound);
        }
    }

    public void PlayCoinSound()
    {
        if (audioSource.enabled)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }
}
