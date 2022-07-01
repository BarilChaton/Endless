using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject theBridge;
    void Start()
    {

    }
    void Update()
    {

    }

    public void Raise()
    {
        theBridge.GetComponent<Bridge01>().RaiseBridge();
        print("bob");
    }
}
