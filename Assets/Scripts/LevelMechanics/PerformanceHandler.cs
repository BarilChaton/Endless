using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceHandler : MonoBehaviour
{
    public GameObject player;

    public GameObject levelPart1;
    public GameObject levelPart2;

    [Header("Use if needed!")]
    public GameObject levelPart3;

    private void OnTriggerEnter(Collider player)
    {
        if (levelPart1.activeInHierarchy)
        {
            levelPart1.SetActive(false);
            levelPart2.SetActive(true);
            levelPart3.SetActive(true);
        }
        else if (!levelPart1.activeInHierarchy)
        {
            levelPart1.SetActive(true);
            levelPart2.SetActive(false);
            levelPart3.SetActive(false);
        }
        
        
    }
}
