/*****************************************************************************
// File Name : CameraMove.cs
// Author : Logan Dagenais
// Creation Date : February 28, 2025
//
// Brief Description : This script positions the camera to the player head
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    // Update is called once per frame
    /// <summary>
    /// updates camera position
    /// </summary>
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
