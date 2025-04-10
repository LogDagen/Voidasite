/*****************************************************************************
// File Name : CameraFollow.cs
// Author : Logan Dagenais
// Creation Date : February 28, 2025
//
// Brief Description : This script gets the mouse and lets it move the camera
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private Transform orientation;
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    private GameManager gameManager;

    /// <summary>
    /// locks cursor to center of screen and hides it
    /// </summary>
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /// <summary>
    /// gets the mouse input and movement to rotate the camera when it moves
    /// </summary>
    private void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * (sensX * PlayerPrefs.GetFloat("SensSetting"));
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (sensY * PlayerPrefs.GetFloat("SensSetting"));

        yRotation += mouseX;
        xRotation += -mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}