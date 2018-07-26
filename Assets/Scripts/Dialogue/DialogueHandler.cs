using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour {

    public DialogueAsset asset;

    [Multiline]
    public string content;
    public TextMeshProUGUI textTemplate;
    public Transform textPanel;
    public Image portrait;

    public float delay = 0.02f;
    int currentDialogueIndex = 0;

    bool dialogueActive = false; 
    private bool dialogueFinished = true;
    bool dialogueOpen = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();   
    }

    private void Update()
    {
        if (dialogueActive)
        {
            if (Input.GetButtonDown("Fire1") && dialogueFinished)
            {
                if (asset.containers.Length - 1 < currentDialogueIndex)
                {
                    CloseDialogue();
                }
                else
                {
                    if (!dialogueOpen)
                    {
                        OpenDialogue();
                    }

                    if (asset.containers[currentDialogueIndex].continueDialogue)
                    {
                        StartDialogue(currentDialogueIndex, asset);
                    }
                    else
                    {
                        dialogueActive = false;
                        CloseDialogue();
                    }
                }
            }
        }
    }

    void OpenDialogue()
    {
        if (!dialogueOpen)
        {
            dialogueOpen = true;
            animator.Play("Open");
        }
    }

    void CloseDialogue()
    {
        if (dialogueOpen)
        {
            dialogueOpen = false;
            animator.Play("Close");
        }
    }

    /// <summary>
    /// Starts dialogue and increments dialogueindex by 1.
    /// </summary>
    /// <param name="startIndex"></param>
    public void StartDialogue(int startIndex, DialogueAsset inAsset)
    {
        dialogueActive = true;

        asset = inAsset;

        OpenDialogue();

        currentDialogueIndex = startIndex;
        content = asset.containers[startIndex].content;

        StartCoroutine(Print());

        currentDialogueIndex++;
    }

    IEnumerator Print()
    {
        dialogueFinished = false;

        yield return new WaitForSeconds(0.5f);

        int childCount = textPanel.childCount;

        for (int i = childCount; i > 0; i--)
        {
            Destroy(textPanel.GetChild(i-1).gameObject);
        }

        bool popupActive = false;
        bool printBold = false;
        int waitMultiplier = 1;

        for (int i = 0; i < content.Length; i++)
        {
            TextMeshProUGUI spawnedLetter = Instantiate(textTemplate, textPanel.transform);

            switch (content[i])
            {
                case '#':
                    printBold = !printBold;
                    i++;
                    break;
                case '*':
                    waitMultiplier = int.Parse(content[i + 1].ToString());
                    i = i + 2;
                    break;
                case '/':
                    popupActive = !popupActive;
                    i++;
                    break;
                default:
                    break;
            }

            spawnedLetter.text = content[i].ToString();
            if (popupActive)
            {
                spawnedLetter.GetComponent<Animator>().enabled = true;
                spawnedLetter.GetComponent<Animator>().Play("PopUp");
            }
            if (printBold)
            {
                spawnedLetter.fontSize = spawnedLetter.fontSize + 8;
            }

            yield return new WaitForSeconds(delay * waitMultiplier);
        }

        dialogueFinished = true;
    }
}
