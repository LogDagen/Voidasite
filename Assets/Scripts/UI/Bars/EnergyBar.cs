/*****************************************************************************
// File Name : EnergyBar.cs
// Author : Logan Dagenais
// Creation Date : March 8, 2025
//
// Brief Description : This code controls the players Energy bar
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private float energy;
    private Slider Energybar;
    private PlayerController Playercontroller;
    /// <summary>
    /// Gets and sets components
    /// </summary>
    void Start()
    {
        Playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Energybar = GetComponent<Slider>();
        Energybar.maxValue = Playercontroller.MaxEnergy;
        Energybar.value = 0;
    }
    /// <summary>
    /// Lowers energy and sets it to bar
    /// </summary>
    /// <param name="drain">energy drained</param>
    public void lowerEnergy(int drain)
    {
        energy -= drain;
        Energybar.value = energy;
    }
    /// <summary>
    /// gains energy and sets it to bar
    /// </summary>
    /// <param name="energyGain">energy gained</param>
    public void GainEnergy(int energyGain)
    {
        energy += energyGain;
        Energybar.value = energy;
    }
    /// <summary>
    /// sets energy and sets it to bar
    /// </summary>
    /// <param name="Energy">Energy</param>
    public void SetEnergy(int Energy)
    {
        energy = Energy;
        Energybar.value = Energy;
    }
}
