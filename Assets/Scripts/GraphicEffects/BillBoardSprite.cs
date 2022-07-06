using UnityEngine;
using Endless.PlayerCore;

public class BillBoardSprite : MonoBehaviour
{
    // This script will be handling so that the sprite object allways look at the player so they never get too thin.

    private SpriteRenderer sr;
    Vector3 target = Vector3.zero;
    [SerializeField] private bool useYPosition = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true;
        Invoke(nameof(GetInfo), 0.2f);
    }

    void Update()
    {
        target = PlayerController.instance.transform.position;
        target = useYPosition ? target : new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(target, Vector3.up);
    }

    private void GetInfo()
    {
        target = PlayerController.instance.transform.position;
        target = useYPosition ? target : new Vector3(target.x, transform.position.y, target.z);
    }
}