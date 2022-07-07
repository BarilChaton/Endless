using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Control;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip roarSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

    public void PlayRoarSound()
    {
        audioSource.PlayOneShot(roarSound);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
}
