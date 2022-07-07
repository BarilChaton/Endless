using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Button : Interactable
{
    public GameObject affectedObject;
    public GameObject affectedObject2;
    public GameObject affectedObject3;
    public Animator anim;

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
        affectedObject.GetComponent<ActivateMovingObject>().startMoving = true;
        affectedObject2.SetActive(false);
        affectedObject3.SetActive(true);
        

    }

    public override void OnLooseFocus()
    {

    }
}
