using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange = 20f;

    private BoxCollider gunTrigger;
    
    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, verticalRange, range);
        gunTrigger.center = new Vector3(0, 0, range * 0.5f);
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Add potential enemy to shoot
    }

    private void OnTriggerExit(Collider other)
    {
        //Remove potential enemy to shoot
    }
}
