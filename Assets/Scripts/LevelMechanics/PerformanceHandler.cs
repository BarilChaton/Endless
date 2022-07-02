using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceHandler : MonoBehaviour
{
    public GameObject levelPart1;
    public GameObject levelPart2;
    public GameObject player;
    
    void Awake()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider player)
    {
        if (levelPart1.active)
        {
            levelPart1.SetActive(false);
            levelPart2.SetActive(true);
        }
        else
        {
            levelPart1.SetActive(true);
            levelPart2.SetActive(false);
        }
        
        
    }
}
