using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    // This script will be used to check whatever the player is looking at and presses the "E" key.
    // If the ray hits an interactable.. something will happen.
    RaycastHit hit;

    void Start()
    {
        
    }


    void Update()
    {
        // test.. if works seperate type of objects interacted with in methods.
        
        
            //if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
            //{
            //    if (Input.GetKeyDown(KeyCode.E))
            //    {
            //        if (hit.collider.tag == "Button")
            //        {
            //            onInteract.Invoke();
            //            Debug.Log("I press at" + hit.collider.name);
            //        }
            //    }
            //}
    }
}
