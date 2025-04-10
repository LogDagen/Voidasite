/*****************************************************************************
// File Name : HealthBar.cs
// Author : Logan Dagenais
// Creation Date : March 8, 2025
//
// Brief Description : This code controls the players health bar
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float Hp;
    private Slider healthBar;
    private PlayerController Health;
    // Start is called before the first frame update
    /// <summary>
    /// Gets and sets components
    /// </summary>
    void Start()
    {
        Health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = Health.MaxHealth;
        healthBar.value = Health.MaxHealth;
    }
    /// <summary>
    /// lowers health and sets it to Health bar
    /// </summary>
    /// <param name="damage">Damage taken</param>
    public void DamageHealth(int damage)
    {
        Hp -= damage;
        healthBar.value = Hp;
    }
    /// <summary>
    /// Regens health and sets it to Health Bar
    /// </summary>
    /// <param name="regen"></param>
    public void HealHealth(int regen)
    {
        Hp += regen;
        healthBar.value = Hp;
    }
    /// <summary>
    /// Sets health to specified value
    /// </summary>
    /// <param name="health">Health</param>
    public void SetHealth(int health)
    {
        Hp = health;
        healthBar.value = Hp;
    }
}
