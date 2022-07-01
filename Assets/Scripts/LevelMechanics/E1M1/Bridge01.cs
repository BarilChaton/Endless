using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge01 : MonoBehaviour
{
    public bool isTriggered = false;
    [SerializeField] private Vector3 target;
    [SerializeField] private float speed = 5;

    private void Update()
    {

    }

    public void RaiseBridge()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        print("dylan");
    }
}
