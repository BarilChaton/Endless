using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Endless.PlayerCore;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBarImage;
    private PlayerController player;

    void Update()
    {
        healthBarImage.fillAmount = player.GetComponent<PlayerCombat>().SetHealthBar();
    }
}
