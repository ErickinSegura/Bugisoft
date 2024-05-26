<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static SFXController instance;
    // Audio Source
    public AudioSource audioSource;

    // Audio Clips
    public AudioClip[] audioClips;

    // Play SFX
    public void PlaySFX(int clipIndex)
    {
        Debug.Log("Playing SFX: " + clipIndex);
        audioSource.clip = audioClips[clipIndex];
        audioSource.Play();

    }

    public void PLayAndStopSFX(int clipIndex)
    {
        Debug.Log("Playing SFX: " + clipIndex);
        audioSource.clip = audioClips[clipIndex];
        audioSource.Play();
        StartCoroutine(StopSFXAfterSeconds(audioClips[clipIndex].length));
    }

    IEnumerator StopSFXAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
    }

    // Stop SFX
    public void StopSFX()
    {
        audioSource.Stop();
    }

    // Pause SFX
    public void PauseSFX()
    {
        audioSource.Pause();
    }

    // Unpause SFX
    public void UnpauseSFX()
    {
        audioSource.UnPause();
    }

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static SFXController instance;
    // Audio Source
    public AudioSource audioSource;

    // Audio Clips
    public AudioClip[] audioClips;

    // Play SFX
    public void PlaySFX(int clipIndex)
    {
        Debug.Log("Playing SFX: " + clipIndex);
        audioSource.clip = audioClips[clipIndex];
        audioSource.Play();

    }

    public void PLayAndStopSFX(int clipIndex)
    {
        Debug.Log("Playing SFX: " + clipIndex);
        audioSource.clip = audioClips[clipIndex];
        audioSource.Play();
        StartCoroutine(StopSFXAfterSeconds(audioClips[clipIndex].length));
    }

    IEnumerator StopSFXAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
    }

    // Stop SFX
    public void StopSFX()
    {
        audioSource.Stop();
    }

    // Pause SFX
    public void PauseSFX()
    {
        audioSource.Pause();
    }

    // Unpause SFX
    public void UnpauseSFX()
    {
        audioSource.UnPause();
    }

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
>>>>>>> eb9587cc21b140b7ddc5c9534f366afe8d70462e
