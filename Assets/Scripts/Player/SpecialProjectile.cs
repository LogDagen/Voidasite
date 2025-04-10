/*****************************************************************************
// File Name : SpecialProjectile.cs
// Author : Logan Dagenais
// Creation Date : March 8, 2025
//
// Brief Description : This code controls the player's special attack
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private Transform Bullet;
    private GameManager gameManager;
    [SerializeField] private LayerMask EnemyMask;
    [SerializeField] private float AttackRange;
    [SerializeField] private float Speed;
    [SerializeField] private int BulletExist;
    // Start is called before the first frame update
    /// <summary>
    /// Gets components
    /// </summary>
    void Start()
    {
        Bullet = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb.AddForce(transform.forward * Speed, ForceMode.Impulse);
        StartCoroutine(Despawn());
    }
    /// <summary>
    /// Makes raycast to destroy all enemys in its way
    /// </summary>
    private void Update()
    {
        //makes raycast to hit enemy when reflected
        Collider[] enemiesToCounter = Physics.OverlapSphere(Bullet.position, AttackRange, EnemyMask);
        for (int i = 0; i < enemiesToCounter.Length; i++)
        {
            enemiesToCounter[i].GetComponentInParent<EnemyHealth>().Death();
            gameManager.Freeze();
        }
    }
    /// <summary>
    /// Destroys itself after some time
    /// </summary>
    /// <returns></returns>
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(BulletExist);
        Destroy(gameObject);
        yield break;
    }
}
