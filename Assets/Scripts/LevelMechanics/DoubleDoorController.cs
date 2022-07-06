using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoorController : Interactable
{
    private bool startOpening = false;
    private bool startClosing = false;
    public bool canBeInteractedWith = true;

    public GameObject Door0;
    public GameObject Door1;

    public Vector3 Door0Target;
    public Vector3 Door0Origin;
    public Vector3 Door1Target;
    public Vector3 Door1Origin;
    public float speed;

    private void Update()
    {
        if (startOpening && Door0.transform.localPosition != Door0Target && Door1.transform.localPosition != Door1Target)
        {
            Door0.transform.localPosition = Vector3.MoveTowards(transform.localPosition, Door0Target, Time.deltaTime * speed);
            Door1.transform.localPosition = Vector3.MoveTowards(transform.localPosition, Door1Target, Time.deltaTime * speed);
            canBeInteractedWith = false;
        }
        if (Door0.transform.localPosition == Door0Target && Door1.transform.localPosition == Door1Target)
        {
            StartCoroutine(AutoClose());
        }
        if (startClosing && Door0.transform.localPosition != Door0Origin && Door1.transform.localPosition != Door1Origin)
        {
            Door0.transform.localPosition = Vector3.MoveTowards(transform.localPosition, Door0Origin, Time.deltaTime * speed);
            Door1.transform.localPosition = Vector3.MoveTowards(transform.localPosition, Door1Origin, Time.deltaTime * speed);
            startClosing = false;
        }
        if (Door0.transform.localPosition == Door0Origin && Door1.transform.localPosition == Door1Origin)
        {
            StopAllCoroutines();
        }
    }

    public override void OnInteract()
    {
        if (canBeInteractedWith)
            startOpening = true;
    }

    public override void OnFocus()
    {

    }

    public override void OnLooseFocus()
    {

    }

    private IEnumerator AutoClose()
    {
        while (!canBeInteractedWith)
        {
            yield return new WaitForSeconds(3);

            startOpening = false;
            startClosing = true;
            canBeInteractedWith = true;
        }
    }
}
