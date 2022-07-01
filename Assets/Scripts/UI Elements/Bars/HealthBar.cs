using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Endless.PlayerCore;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private Image healthBarImage;
    private PlayerCombat player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCombat>();
        healthBarImage = GetComponent<Image>();
    }

    public void Update()
    {
        healthBarImage.fillAmount = player.SetHealthBar() / 100;
    }
}
