using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elavator : MonoBehaviour
{
    // 2.81 seems to be the sweet spot for this one. Will have to recalculate if you want another floor.
    [SerializeField] private Vector3 floorDistance = new Vector3(0, 2.18f, 0);

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float returnTimer = 10f;
    [SerializeField] private float activationTimer = 0f;
    [SerializeField] private int floor = 0;
    [SerializeField] private int maxFloor = 1;

    // In case you want something ELSE to move
    [SerializeField] private Transform moveTransform;

    private float tTotal;
    private bool isMoving;
    private float moveDirection;
    private float invokeTimer;

    void Start()
    {
        moveTransform = moveTransform != null ? moveTransform : transform;
    }

    void Update()
    {
        // If ismoving is activated, we move
        if (isMoving)
        {
            MoveElevator();
        }
    }

    void MoveElevator()
    {
        // Setting the move directions
        // I found it easier to just move it x distance instead of giving it destinations
        Vector3 v = moveDirection * floorDistance * speed;
        float t = Time.deltaTime;
        float tMax = floorDistance.magnitude / speed;
        t = Mathf.Min(t, tMax - tTotal);

        // Actually moving
        moveTransform.Translate(v * t);
        tTotal += t;

        // landing
        if (tTotal >= tMax)
        {
            isMoving = false;
            tTotal = 0;
        }
    }

    public void StartMoveUp()
    {
        // so you can't restart move as it moves
        if (isMoving) return;

        // Starting return timer
        if (returnTimer != 0)
        {
            Invoke("StartMoveDown", returnTimer);
        }

        // movement starts, up or down (up here), and add/remove a floor
        isMoving = true;
        moveDirection = 1;
        floor += 1;
    }

    public void StartMoveDown()
    {
        // same as above idk what you want lmao
        if (isMoving) return;

        isMoving = true;
        moveDirection = -1;
        floor -= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        // You know this part :stuck_out_tongue:
        if (other.tag == "Player" && invokeTimer < Time.time)
        {
            other.transform.parent = this.transform;
            if (floor != maxFloor)
            {
                Invoke(nameof(StartMoveUp), activationTimer);
                invokeTimer = Time.time + returnTimer;
            }
            else if (floor != 0) Invoke(nameof(StartMoveDown), activationTimer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

}