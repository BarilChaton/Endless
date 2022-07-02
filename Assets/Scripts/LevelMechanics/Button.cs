using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    public GameObject affectedObject;
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {

        affectedObject.GetComponent<Bridge01>().startMoving = true;
    }

    public override void OnLooseFocus()
    {

    }
}
