using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    public GameObject affectedObject;
    public GameObject affectedObject2;
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        affectedObject.GetComponent<ActivateMovingObject>().startMoving = true;
        affectedObject2.SetActive(false);
    }

    public override void OnLooseFocus()
    {

    }
}
