using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;

public class BillBoardSprite : MonoBehaviour
{
    // This script will be handling so that the sprite object allways look at the player so they never get too thin.

    private SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true;
    }

    void Update()
    {
        transform.LookAt(PlayerController.instance.transform.position, Vector3.up);
    }
}