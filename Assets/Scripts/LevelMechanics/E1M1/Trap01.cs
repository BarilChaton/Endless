using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap01 : MonoBehaviour
{
    public bool isActive = true;

    private void Update()
    {
        if (!isActive)
        {
            this.enabled = false;
        }
    }
}
