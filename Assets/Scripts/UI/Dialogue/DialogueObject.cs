/*****************************************************************************
// File Name : DialogueObject.cs
// Author : Logan Dagenais
// Creation Date : March 28, 2025
//
// Brief Description : This code creates an Object in unity that lets user
create their dialogue to be said
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//creates a menu item in the Asset menu to create instances of the scriptable object
[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue Data")]
public class DialogueObject : ScriptableObject
{
    public string[] lines;
}
