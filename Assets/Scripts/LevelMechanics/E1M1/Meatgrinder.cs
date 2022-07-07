using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;
public class Meatgrinder : MonoBehaviour
{
    // Hi! My name is MeatGrinder. I grind meat. I love doing it. Especially "Player" meat. NomNomNom!
    [SerializeField] int damageAmount = 100;
    [SerializeField] GameObject Player;

    private void OnTriggerEnter(Collider Player)
    {
        int amount = 0;
        {
            if (amount == 0) amount = damageAmount;
            Player.GetComponent<PlayerCombat>().PlayerTakeDamage(amount);
            // I think this should be correct format.
            //Destroy(this.gameObject);
        }
        //else
        //{
        //    Destroy(this.gameObject, 4f);
        //}
        //return;
    }

}
