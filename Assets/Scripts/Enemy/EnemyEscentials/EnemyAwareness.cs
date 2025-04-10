/*****************************************************************************
// File Name : EnemyAwareness.cs
// Author : Logan Dagenais
// Creation Date : March 3, 2025
//
// Brief Description : Aggrivates the Enemy and lets them attack the player
when close enough
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    [SerializeField] private float awarenessRadius;

    public bool isAggro;
    private Transform PlayerTransform;
    [SerializeField] private Transform FirePoint;
    /// <summary>
    /// Gets transform from Player
    /// </summary>
    private void Start()
    {
        PlayerTransform = FindObjectOfType<PlayerController>().transform;
    }
    /// <summary>
    /// enables monster to attack player when close enough
    /// </summary>
    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(FirePoint.position, FirePoint.transform.TransformDirection(Vector3.forward), out hit, awarenessRadius))
        {
            if (hit.transform.gameObject.tag == "Player" && isAggro == false)
            {
                Aggrivate();
            }
        }
    }
    /// <summary>
    /// sets aggro to true, is then used for other scripts
    /// </summary>
    public void Aggrivate()
    {
        isAggro = true;
    }
}
