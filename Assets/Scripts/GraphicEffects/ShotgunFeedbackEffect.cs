using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunFeedbackEffect : MonoBehaviour
{
    public float lifeTime;

    void Start()
    {
    }
    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
