/*****************************************************************************
// File Name : KeyCard.cs
// Author : Logan Dagenais
// Creation Date : March 25, 2025
//
// Brief Description : This code controls the levels in the title screen
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject levelButtons;
    /// <summary>
    /// displays buttons
    /// </summary>
    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0;i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    /// <summary>
    /// opens selected level
    /// </summary>
    /// <param name="levelID">level</param>
    public void OpenLevel(int levelID)
    {
        string levelName = "Level " + levelID;
        SceneManager.LoadScene(levelName);
    }
    /// <summary>
    /// gets buttons in gameobject
    /// </summary>
    void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }
    public void ResetLevel()
    {
        PlayerPrefs.SetInt("ReachedIndex", 1);
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();
    }
}
