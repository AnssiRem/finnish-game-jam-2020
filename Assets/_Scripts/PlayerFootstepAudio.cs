using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootstepAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] FootstepClips;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void StepSFX()
    {
        source.clip = FootstepClips[Random.Range(0, FootstepClips.Length)];
        source.Play();
    }
}

