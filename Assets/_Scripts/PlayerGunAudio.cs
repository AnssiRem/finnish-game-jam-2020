using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerGunAudio : MonoBehaviour
{
    [Header("Audio Clip References")]
#pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private AudioClip gunStart;
    [SerializeField] private AudioClip gunLoop;
    [SerializeField] private AudioClip gunEnd;
#pragma warning restore IDE0044 // Add readonly modifier

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void StartShootSFX()
    {
        source.clip = gunStart;
        source.Play();
        Invoke("StartLoopSFX", gunStart.length);
    }

    private void StartLoopSFX()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            source.clip = gunLoop;
            source.loop = true;
            source.Play(); 
        }
        else
        {
            source.Stop();
        }
    }

    public void StopShootSFX()
    {
        if(source.clip == gunStart)
        {
            source.Stop();
        }
        else if(source.clip == gunLoop)
        {
            source.clip = gunEnd;
            source.loop = false;
            source.Play(); 
        }
    }
}
