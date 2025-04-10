/*****************************************************************************
// File Name : StaminaBar.cs
// Author : Logan Dagenais
// Creation Date : March 8, 2025
//
// Brief Description : This code controls the players Stamina bar
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private float Stamin;
    private Slider Staminabar;
    private PlayerController Stamina;
    // Start is called before the first frame update
    /// <summary>
    /// Gets and sets components
    /// </summary>
    void Start()
    {
        Stamina = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Staminabar = GetComponent<Slider>();
        Staminabar.maxValue = Stamina.MaxDashStamina;
        Staminabar.value = Stamina.MaxDashStamina;
    }
    /// <summary>
    /// lowers health and sets it to Health bar
    /// </summary>
    /// <param name="drain">Stamina taken</param>
    public void LowerStamina(int drain)
    {
        Stamin -= drain;
        Staminabar.value = Stamin;
    }
    /// <summary>
    /// Regens health and sets it to Health Bar
    /// </summary>
    /// <param name="regen">Staminagained</param>
    public void HealStamina(int regen)
    {
        Stamin += regen;
        Staminabar.value = Stamin;
    }
    /// <summary>
    /// Sets health to specified value
    /// </summary>
    /// <param name="stamina">Stamina set</param>
    public void SetStamina(int stamina)
    {
        Stamin = stamina;
        Staminabar.value = Stamin;
    }
}