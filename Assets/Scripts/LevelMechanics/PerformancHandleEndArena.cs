using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformancHandleEndArena : MonoBehaviour
{
    public GameObject player;

    public GameObject levelPart01;
    public GameObject levelPart02;
    public GameObject levelPart03;
    public GameObject levelPart04;

    private void OnTriggerEnter(Collider player)
    {
        if (levelPart01.activeInHierarchy || levelPart02.activeInHierarchy || levelPart03.activeInHierarchy || levelPart04.activeInHierarchy)
        {
            levelPart01.SetActive(false);
            levelPart02.SetActive(false);
            levelPart03.SetActive(false);
            levelPart04.SetActive(false);
        }
        else if (!levelPart01.activeInHierarchy || !levelPart02.activeInHierarchy || !levelPart03.activeInHierarchy || !levelPart04.activeInHierarchy)
        {
            levelPart01.SetActive(true);
            levelPart02.SetActive(true);
            levelPart03.SetActive(true);
            levelPart04.SetActive(true);
        }
    }
}
