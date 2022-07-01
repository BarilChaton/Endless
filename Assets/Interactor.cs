using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    public LayerMask interactableLayerMask = 11;
    UnityEvent onInteract;

    void Start()
    {

    }
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, interactableLayerMask))
        {
            print(hit.collider.name);
            if (hit.collider.GetComponent<Interactable>() != false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.GetComponent<Interactable>().Raise();
                }
            }
            
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red);
    }
}
