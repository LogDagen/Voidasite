/*****************************************************************************
// File Name : CardReader.cs
// Author : Logan Dagenais
// Creation Date : March 25, 2025
//
// Brief Description : This code controls the key card readers to open the door
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReader : MonoBehaviour
{
    private GameObject Door;
    /// <summary>
    /// gets components
    /// </summary>
    private void Start()
    {
        Door = transform.GetChild(0).gameObject;
    }
    /// <summary>
    /// Opens door
    /// </summary>
    public void CardRead()
    {
        Door.SetActive(false);
    }
}
