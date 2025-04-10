/*****************************************************************************
// File Name : Dialogue.cs
// Author : Logan Dagenais
// Creation Date : March 28, 2025
//
// Brief Description : This code activates when the player collides with the
object and starts dialogue
*****************************************************************************/
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivate : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogue;
    private Dialogue DialogueBox;
    [SerializeField] private bool DestroyOnCollision;
    private void Awake()
    {
        DialogueBox = FindObjectOfType<Dialogue>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueBox.lines = dialogue.lines;
            DialogueBox.StartDialogue();
        }
        if (DestroyOnCollision)
        {
            Destroy(gameObject);
        }
    }
}
