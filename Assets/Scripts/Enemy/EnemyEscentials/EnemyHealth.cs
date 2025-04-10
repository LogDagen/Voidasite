/*****************************************************************************
// File Name : EnemyHealth.cs
// Author : Logan Dagenais
// Creation Date : March 1, 2025
//
// Brief Description : This code controls the enemies health, adds damage
Particles and controls its Sprite
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyHealth : MonoBehaviour
{
    private PlayerController playercontroller;
    private GameManager gamemanager;
    [SerializeField] private int energy;
    [SerializeField] private bool HasKeyCard;
    private Animator SpriteAnim;
    private AlignToPlayer angleToplayer;
    [SerializeField] private int health;
    [SerializeField] private int Regen;
    [SerializeField] private int Points;
    [SerializeField] private GameObject GunHitEffect;
    [SerializeField] private GameObject keyCard;
    /// <summary>
    /// gets Animator and Align to player script and gamemanager
    /// </summary>
    private void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SpriteAnim = GetComponentInChildren<Animator>();
        angleToplayer = GetComponent<AlignToPlayer>();
    }
    /// <summary>
    /// sets the sprite to the index from the Align script
    /// </summary>
    private void Update()
    {
        SpriteAnim.SetFloat("spriteRot", angleToplayer.lastIndex);
    }
    /// <summary>
    /// damages enemy when hit,summons and destroys particle, and destroy itself if the health is zero or less
    /// </summary>
    /// <param name="damage">damage recieved from player</param>
    public void Damage (int damage)
    {
        //creates gun hit effect when hit
        GameObject a = Instantiate (GunHitEffect, transform.position, Quaternion.identity);
        //damages self
        health -= damage;
        //destroys itself if it is defeated
        if (health <= 0)
        {
            if (HasKeyCard)
            {
                Instantiate(keyCard, transform.position, Quaternion.identity);
            }
            Death();
        }
        //destroys particle
        Destroy (a, 1);
    }
    /// <summary>
    /// destroys enemy and gives points
    /// </summary>
    public void Death()
    {
        gamemanager.UpdateScore(Points);
        GameObject a = Instantiate(GunHitEffect, transform.position, Quaternion.identity);
        playercontroller.GainEnergy(energy);
        playercontroller.Heal(Regen);
        Destroy (gameObject);
        Destroy(a, 1);
    }
}
