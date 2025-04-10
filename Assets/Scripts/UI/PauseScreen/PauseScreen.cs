/*****************************************************************************
// File Name : PauseScreen.cs
// Author : Logan Dagenais
// Creation Date : March 11, 2025
//
// Brief Description : This code controls the pause screen and its functions
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    private GameManager gameManager;
    /// <summary>
    /// gets components
    /// </summary>
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    /// <summary>
    /// resumes game
    /// </summary>
    public void Resume()
    {
        gameManager.UnpauseGame();
    }
    /// <summary>
    /// restarts game
    /// </summary>
    public void Restart()
    {
        gameManager.RestartLevel();
        
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        gameManager.Quitlevel();
    }
}
