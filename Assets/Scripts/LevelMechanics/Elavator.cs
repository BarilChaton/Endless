using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elavator : MonoBehaviour
{
    [SerializeField] private Vector3 origin, target;
    [SerializeField] private float speed;
    [SerializeField] private float setTime = 2;

    private float timer;
    private bool timeToGo;
    private bool goingUp = false;
    private bool playerOnTop = false;

    private void Awake()
    {
        timer = setTime;
    }

    private void FixedUpdate()
    {
        if (!goingUp && playerOnTop)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position == target)
            {
                goingUp = true;
                Invoke("TimeToGoBack", setTime);
            }
        }
        else if (transform.position == origin)
        {
            goingUp = false;
        }
        //else if (goingUp && playerOnTop && timeToGo)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, origin, +speed * Time.deltaTime);
        //    if (transform.position == origin)
        //    {
        //        goingUp = false;
        //    }
        //}
        //else if (timeToGo)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, origin, +speed * Time.deltaTime);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerOnTop = true;
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    private void TimeToGoBack()
    {
        playerOnTop = false;
        goingUp = false;
        transform.position = Vector3.MoveTowards(transform.position, origin, speed * Time.deltaTime);
    }
}
