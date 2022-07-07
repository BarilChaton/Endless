using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoorControl : Interactable
{
    public GameObject affectedObject;
    private Animator anim;

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
        affectedObject.GetComponent<DoorController>().isUnlocked = true;
    }

    public override void OnLooseFocus()
    {

    }

    
}
