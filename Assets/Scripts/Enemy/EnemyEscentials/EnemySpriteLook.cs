/*****************************************************************************
// File Name : EnemySpriteLook.cs
// Author : Logan Dagenais
// Creation Date : March 4, 2025
//
// Brief Description : This code lets the enemy face the Player, and can
choose if it can turn vertically or just Horizontally
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteLook : MonoBehaviour
{
    private Transform target;
    [SerializeField] private bool canLookVertically;
    // Start is called before the first frame update
    /// <summary>
    /// Gets transform from player
    /// </summary>
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    /// <summary>
    /// looks at target and restrains it if it cant vertically
    /// </summary>
    void Update()
    {
        if (canLookVertically)
        {
            transform.LookAt(target);
        }
        else
        {
            Vector3 modifiedTarget = target.position;
            modifiedTarget.y = transform.position.y;
            transform.LookAt(modifiedTarget);
        }
    }
}
