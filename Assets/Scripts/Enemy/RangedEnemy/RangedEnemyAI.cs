/*****************************************************************************
// File Name : RangedEnemyAI.cs
// Author : Logan Dagenais
// Creation Date : March 6, 2025
//
// Brief Description : This code controls the Ranged enemies AI to shoot
the player
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class RangedEnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    private Transform Player;
    private Rigidbody rb;
    private EnemyAwareness enemyAwareness;
    [SerializeField] private Transform bulletSpawnPoint;
    private bool hasAggro;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float Speed;
    [SerializeField] private float FireCooldown;
    /// <summary>
    /// gets script and transform
    /// </summary>
    private void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        Player = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// lets enemy turn and attack when aggrivated
    /// </summary>
    private void Update()
    {
        if (enemyAwareness.isAggro)
        {
            Vector3 relativePos = Player.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, TurnSpeed);
            //only lets couroutine happen once
            if (hasAggro == false)
            {
                StartCoroutine(Fire());
                hasAggro = true;
            }
            if ((transform.position - Player.transform.position).magnitude > 60.0f)
            {
                transform.position = Vector3.MoveTowards(rb.position, Player.transform.position,
                Speed * Time.deltaTime);
            }
        }
        if ((transform.position - Player.transform.position).magnitude < 10.0f)
        {
            transform.position = Vector3.MoveTowards(rb.position, Player.transform.position,
            -Speed * Time.deltaTime);
        }
    }
    /// <summary>
    /// Fires bullet and waits before repeating
    /// </summary>
    /// <returns>Cooldown</returns>
    IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(FireCooldown);
            Shoot();
        }
    }
    /// <summary>
    /// shoots bullet infront of itself
    /// </summary>
    private void Shoot()
    {
        Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
