using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Control;
using Random = UnityEngine.Random;


namespace Endless.PlayerCore
{
    public class GunCore : MonoBehaviour
    {
        [Header("Shooty Stuff")]
        [SerializeField] public float DamageAmt;
        [SerializeField] public float ShotCooldown = 1f;
        [HideInInspector] public float CurrentCD;
        [Header("Ammunation")]
        [SerializeField] public int MaxAmmo;
        [HideInInspector] public int AmmoPerClip;
        [SerializeField] public int CurrentTotalAmmo;
        [HideInInspector] public int CurrentAmmo;
        [Header("Gun Details")]
        [SerializeField] public bool spreadShot = true;
        [SerializeField] public float spreadShotWidth = 0.3f;
        [SerializeField] public int spreadShotNumber = 1;
        [Header("Art stuff")]
        [SerializeField] public GameObject wallHitImpact;
        public Animator gunAnim;


        public void ShootGun(Camera playerCamera = null)
        {
            if (playerCamera != null)
            {
                if (CurrentTotalAmmo > 0)
                {
                    Ray[] rays;
                    if (spreadShot) rays = SpreadShotStuff(playerCamera);
                    else rays = NotSpread(playerCamera);

                    // Checking if it hit anything
                    foreach (Ray r in rays)
                    {
                        if (Physics.Raycast(r, out RaycastHit hit))
                        {
                            // Hitting an enemy specifically
                            if (hit.transform.tag == "Enemy")
                            {
                                float damage = DamageAmt;
                                hit.collider.GetComponent<EnemyCore>().TakeDamage(damage);
                                Instantiate(hit.collider.GetComponent<EnemyCore>().hurtImpact, hit.point, playerCamera.transform.rotation);
                            }

                            // Hitting anything else. TODO: Hitting decoration? Lamps? -- TBI
                            else
                            {
                                Instantiate(wallHitImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                            }
                        }


                        // Hit fuckall
                        else
                        {
                            Debug.Log("I'm looking at nothing");
                        }
                    }
                    // Removing ammo and doing animations
                    CurrentTotalAmmo--;
                    gunAnim.Play("Shoot");
                }
            }
        }

        public void ShootMachineGun(Camera playerCamera = null)
        {

            // ratatatatatattatatata

        }

        private Ray[] SpreadShotStuff(Camera playerCamera = null)
        {

            Vector2[] points = new Vector2[spreadShotNumber];
            GaussianDistribution gd = new GaussianDistribution(); // maybe send a Random state through the ctor? I don't really use Unity's random any more
            for (int i = 0; i < spreadShotNumber; i++)
            {
                points[i] = new Vector2(Random.Range(-spreadShotWidth, spreadShotWidth), Random.Range(-spreadShotWidth, spreadShotWidth));
            }
            Vector3 direction = playerCamera.transform.forward;
            Vector3 spread = Vector3.zero;


            Ray[] rays = new Ray[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                spread += playerCamera.transform.up * Random.Range(-spreadShotWidth, spreadShotWidth);
                spread += playerCamera.transform.right * Random.Range(-spreadShotWidth, spreadShotWidth);
                direction += spread.normalized * Random.Range(0, spreadShotWidth); // Fixing the normalised aspect to make it more circular
                rays[i] = new Ray(playerCamera.transform.position, direction);
            }
            return rays;
        }

        private Ray[] NotSpread(Camera playerCamera = null)
        {
            Ray[] ray = new Ray[1];
            ray[0] = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            return ray;
        }


    }
}