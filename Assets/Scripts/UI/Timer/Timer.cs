/*****************************************************************************
// File Name : Timer.cs
// Author : Logan Dagenais
// Creation Date : March 27, 2025
//
// Brief Description : This code controls the timer
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public float elapsedTime;

    // Update is called once per frame
    /// <summary>
    /// counts the timer up every second
    /// </summary>
    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
