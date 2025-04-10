/*****************************************************************************
// File Name : EnemyAI.cs
// Author : Logan Dagenais
// Creation Date : March 3rd, 2025
//
// Brief Description : This code controls the enemy to move across the
navmesh and attack when aggro
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask PlayerMask;
    private EnemyAwareness enemyAwareness;
    private Transform playerTransform;
    private NavMeshAgent enemynavMeshAgent;
    [SerializeField] private int damage;
    private PlayerController playerController;
    private Transform MeleeEnemyPos;
    [SerializeField] private float attackRangeX;
    [SerializeField] private float attackRangeY;
    [SerializeField] private float attackRangeZ;
    [SerializeField] private bool canAttack;
    [SerializeField] private float attackCooldown;
    /// <summary>
    /// gets scripts and transforms aswell as making bool true
    /// </summary>
    private void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
        enemynavMeshAgent = GetComponent<NavMeshAgent>();
        MeleeEnemyPos = GetComponent<Transform>();
        canAttack = true;
    }
    /// <summary>
    /// Cooldown for attack
    /// </summary>
    /// <returns>Wait time</returns>
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        yield break;
    }
    /// <summary>
    /// makes enemy aggro and attack the player
    /// </summary>
    private void Update()
    {
        //makes enemy go towards player when aggro
        if (enemyAwareness.isAggro)
        {
            enemynavMeshAgent.SetDestination(playerTransform.position);
        }
        else
        {
            enemynavMeshAgent.SetDestination(transform.position);
        }
        //Creates raycast to hit player with
        Collider[] enemiesToDamage = Physics.OverlapBox(MeleeEnemyPos.position, new Vector3(attackRangeX, attackRangeY, attackRangeZ), MeleeEnemyPos.rotation, PlayerMask);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (canAttack == true)
            {
                //Attacks player and starts cooldown
                canAttack = false;
                enemiesToDamage[i].GetComponentInParent<PlayerController>().Damage(damage);
                StartCoroutine(AttackCooldown());
            }
        }
    }
}
