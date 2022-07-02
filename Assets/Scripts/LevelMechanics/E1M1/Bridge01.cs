using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge01 : MonoBehaviour
{
    public bool startMoving = false;
    [SerializeField] private Vector3 target;
    [SerializeField] private float speed = 5;

    private void Update()
    {
        if (startMoving == true && transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }

        if (transform.position == target)
        {
            startMoving = false;
        }
    }

    //public void RaiseBridge()
    //{
    //    if (startMoving)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    //        startMoving = false;
    //    }
    //}
}
