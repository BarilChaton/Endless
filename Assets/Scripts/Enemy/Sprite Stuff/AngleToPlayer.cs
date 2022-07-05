using Endless.PlayerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleToPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 targetPos;
    private Vector3 targetDir;

    private SpriteRenderer spriteRenderer;

    private float angle;
    public int lastIndex;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(AngleRoutine());
    }

    private IEnumerator AngleRoutine()
    {
        WaitForSeconds wait = new(0.1f);

        while (true)
        {
            yield return wait;
            AngleChecker();
        }
    }

    void AngleChecker()
    {
        // Get target position and direction
        targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        targetDir = targetPos - transform.position;

        // Get Angle.
        angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        lastIndex = GetIndex(angle);

        // Flip Sprite.
        // Not all SpriteSheets have 8 angle sprites, sometimes we need to reuse same 4 angle sprites and flip them instead.
        // So instead of complicating stuff we will only use 4 angle sprites on all Spritesheets.


        //Vector3 tempScale = Vector3.one;
        //if (angle > 0)
        //{
        //    tempScale.x *= -1f;
        //}

        //spriteRenderer.transform.localScale = tempScale;
    }

    private int GetIndex(float angle)
    {
        // Front
        if (angle > -22.5f && angle < 22.5f)
            return 0;
        if (angle >= 22.5f && angle < 67.5f)
            return 7;
        if (angle >= 67.5f && angle < 112.5f)
            return 6;
        if (angle >= 112.5f && angle < 157.5f)
            return 5;

        // Back
        if (angle <= -157.5f || angle >= 157.5f)
            return 4;
        if (angle >= -157.5f && angle < -112.5f)
            return 3;
        if (angle >= -112.5f && angle < -67.5f)
            return 2;
        if (angle >= -67.5f && angle <= -22.5f)
            return 1;

        return lastIndex;
    }

    private void OnDrawGizmosSelected()
    {
        /*        Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, targetPos);*/
    }
}
