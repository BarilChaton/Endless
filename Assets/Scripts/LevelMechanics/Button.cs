using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    public GameObject affectedObject;
    public GameObject affectedObject2;
    public GameObject affectedObject3;
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        anim.SetBool("IsPressed", true);
        affectedObject.GetComponent<ActivateMovingObject>().startMoving = true;
        affectedObject2.SetActive(false);
        affectedObject3.SetActive(true);
    }

    public override void OnLooseFocus()
    {

    }
}
