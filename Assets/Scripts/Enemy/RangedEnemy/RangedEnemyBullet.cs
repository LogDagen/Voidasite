/*****************************************************************************
// File Name : RangedEnemyBullet.cs
// Author : Logan Dagenais
// Creation Date : March 6, 2025
//
// Brief Description : This code controls the bullet to move, hit, damage the
player and enemy if it gets reflected by the player, and destroys itself.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBullet : MonoBehaviour
{
    private Transform Bullet;
    private Transform Campos;
    [SerializeField] private float AttackRange;
    [SerializeField] private float Speed;
    [SerializeField] private float BulletExist;
    [SerializeField] private LayerMask PlayerMask;
    [SerializeField] private LayerMask EnemyMask;
    [SerializeField] private int damage;
    private Rigidbody rb;
    private bool hasBeenReflected;
    // Start is called before the first frame update
    /// <summary>
    /// Gets components and starts coroutine
    /// </summary>
    void Start()
    {
        Bullet = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Speed, ForceMode.Impulse);
        Campos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        StartCoroutine(Despawn());
    }
    /// <summary>
    /// creates raycast to hit Player and enemies if it got reflected
    /// </summary>
    private void Update()
    {
        //makes raycast to hit player
        Collider[] enemiesToDamage = Physics.OverlapSphere(Bullet.position, AttackRange, PlayerMask);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            //damages player and destroys itself
            enemiesToDamage[i].GetComponentInParent<PlayerController>().Damage(damage); 
            Destroy(gameObject);
        }
        //makes raycast to hit enemy when reflected
        Collider[] enemiesToCounter = Physics.OverlapSphere(Bullet.position, AttackRange, EnemyMask);
        for (int i = 0; i < enemiesToCounter.Length; i++)
        {
            if (hasBeenReflected == true)
            {
                enemiesToCounter[i].GetComponentInParent<EnemyHealth>().Damage(damage * 2);
                Destroy(gameObject);
            }
        }
    }
    /// <summary>
    /// destroys itself if the bullet hasnt been destroyed for some time
    /// </summary>
    /// <returns>wait time</returns>
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(BulletExist);
        Destroy(gameObject);
        yield break;
    }
    /// <summary>
    /// reverses velocity when reflected
    /// </summary>
    public void reflect()
    {
        rb.velocity = Campos.forward * Speed;
        hasBeenReflected = true;
    }
    /// <summary>
    /// destroys on collision
    /// </summary>
    /// <param name="collision">Anything</param>
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
