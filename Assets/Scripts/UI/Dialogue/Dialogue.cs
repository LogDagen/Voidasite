/*****************************************************************************
// File Name : Dialogue.cs
// Author : Logan Dagenais
// Creation Date : March 28, 2025
//
// Brief Description : This code controls the dialogue Box, displaying text and activating / unactivating
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Diagnostics.Tracing;
public class Dialogue : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI textComponent;
    public string[] lines;
    [SerializeField] private float textSpeed;
    private int index;
    // Start is called before the first frame update
    /// <summary>
    /// sets components
    /// </summary>
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }
    /// <summary>
    /// displays the next text or fills text box if text hasnt displayed yet
    /// </summary>
    public void ReadDialogue()
    {
        if (textComponent.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }
    /// <summary>
    /// displays next line. If there is no line then continues game
    /// </summary>
    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            playerController.IsReading = false;
            textComponent.text = string.Empty;
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
    /// <summary>
    /// starts dialogue when called
    /// </summary>
    public void StartDialogue()
    {
        playerController.IsReading = true;
        gameObject.SetActive(true);
        Time.timeScale = 0;
        index = 0;
        StartCoroutine(TypeLine());
    }
    /// <summary>
    /// displays text left to right
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }
}
