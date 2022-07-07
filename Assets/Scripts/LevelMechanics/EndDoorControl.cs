using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class EndDoorControl : Interactable
{
    public GameObject affectedObject;
    private Animator anim;

    private AudioSource audioSource;
    public AudioClip pressSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        anim.SetBool("IsPressed", true);
        audioSource.PlayOneShot(pressSound);
        affectedObject.GetComponent<DoorController>().isUnlocked = true;
    }

    public override void OnLooseFocus()
    {

    }

    
}
