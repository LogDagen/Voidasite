/*****************************************************************************
// File Name : KeyCard.cs
// Author : Logan Dagenais
// Creation Date : March 25, 2025
//
// Brief Description : This code controls the key card when it interacts with the player
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour
{
    private PlayerController playercontroller;
    // Start is called before the first frame update
    void Start()
    {
        playercontroller = FindObjectOfType<PlayerController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playercontroller.HasKeyCard = true;
            Destroy(gameObject);
        }
    }
}
