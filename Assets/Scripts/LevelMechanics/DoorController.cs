using Endless.PlayerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class DoorController : Interactable
{
    private bool startOpening = false;
    private bool startClosing = false;
    public bool isUnlocked = true;
    public bool canBeInteractedWith = true;
    public Vector3 target;
    public Vector3 origin;
    public float speed;

    private AudioSource audioSource;
    public AudioClip openSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isUnlocked)
        {
            if (startOpening && transform.localPosition != target)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * speed);
                AudioSource.PlayClipAtPoint(openSound, transform.position);
                canBeInteractedWith = false;
            }
            if (transform.localPosition == target)
            {
                StartCoroutine(AutoClose());
            }
            if (startClosing && transform.localPosition != origin)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, origin, Time.deltaTime * speed);
                AudioSource.PlayClipAtPoint(openSound, transform.position);
                startClosing = false;
            }
            if (transform.localPosition == origin)
            {
                StopAllCoroutines();
            }
        }
    }

    public override void OnFocus()
    {
        
    }

    public override void OnInteract()
    {
        if (canBeInteractedWith)
            startOpening = true;
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
