/*****************************************************************************
// File Name : SettingsController.cs
// Author : Logan Dagenais
// Creation Date : April 2, 2025
//
// Brief Description : This script controlls the players music and sensitivity
settings
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private TMP_Text MusicText;
    private TMP_Text SensText;
    private Slider MusicSlider;
    private Slider SensSlider;
    /// <summary>
    /// Gets assets
    /// </summary>
    private void Awake()
    {
        MusicText = GameObject.Find("MusicSlider").GetComponentInChildren<TMP_Text>();
        SensText = GameObject.Find("SensSlider").GetComponentInChildren<TMP_Text>();
        MusicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        SensSlider = GameObject.Find("SensSlider").GetComponent <Slider>();
    }
    /// <summary>
    /// Sets values
    /// </summary>
    private void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicSetting");
        SensSlider.value = PlayerPrefs.GetFloat("SensSetting");
        SensText.text = "Sensitivity: " + PlayerPrefs.GetFloat("SensSetting").ToString("f2");
        MusicText.text = "Music: " + PlayerPrefs.GetFloat("MusicSetting");
    }
    /// <summary>
    /// Changes music setting when changed
    /// </summary>
    public void OnChangeMusic()
    {
        PlayerPrefs.SetFloat("MusicSetting", MusicSlider.value);
        MusicText.text = "Music: " + PlayerPrefs.GetFloat("MusicSetting");
    }
    /// <summary>
    /// Changes Sensitivity Setting When changed
    /// </summary>
    public void OnChangeSens()
    {
        PlayerPrefs.SetFloat("SensSetting", SensSlider.value);
        SensText.text = "Sensitivity: " + PlayerPrefs.GetFloat("SensSetting").ToString("f2");
    }
}
