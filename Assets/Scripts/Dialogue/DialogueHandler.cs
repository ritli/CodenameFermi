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
    private AudioClipPlayer audioPlayer;
    Sprite[] noiseSprites;

    [HideInInspector]
    public GameObject trigger;

    DialoguePreview[] previewTriggers;

    private void Start()
    {
        noiseSprites = Resources.LoadAll<Sprite>("Portraits/Noise/");
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioClipPlayer>();
    }

    private void Update()
    {
        if (dialogueActive)
        {
            if (Input.GetButtonDown("Fire1") && dialogueFinished)
            {
                if (asset.containers[currentDialogueIndex-1].playTriggerAtEndInstead)
                {
                    for (int i = 0; i < asset.containers[currentDialogueIndex-1].triggers.Length; i++)
                    {
                        GameObject g = Instantiate(asset.containers[currentDialogueIndex - 1].triggers[i]);
                        g.GetComponent<ITrigger>().StartTrigger();
                    };

                    if (previewTriggers.Length-1 <= currentDialogueIndex-1)
                    {
                        for (int i = 0; i < previewTriggers[currentDialogueIndex-1].triggers.Length; i++)
                        {
                            previewTriggers[currentDialogueIndex - 1].triggers[i].GetComponent<ITrigger>().StartTrigger();
                        }
                    }
                }

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

                    if (!asset.containers[currentDialogueIndex-1].endDialogue)
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
            Manager.GetPlayer.InDialogue = false;
            Manager.GetPlayer.disableInput = false;

            previewTriggers = null;

            if (trigger)
            {
                trigger.GetComponent<ITrigger>().OnEventFinished();
                trigger = null;
            }
        }
    }

    public void StartDialogue(int startIndex, DialogueAsset inAsset, DialoguePreview[] preview)
    {
        previewTriggers = preview;

        StartDialogue(startIndex, inAsset);
    }

    /// <summary>
    /// Starts dialogue and increments dialogueindex by 1.
    /// </summary>
    /// <param name="startIndex"></param>
    public void StartDialogue(int startIndex, DialogueAsset inAsset)
    {
        dialogueActive = true;

        if (!inAsset.containers[startIndex].playTriggerAtEndInstead)
        {
            for (int i = 0; i < inAsset.containers[startIndex].triggers.Length; i++)
            {
                GameObject g = Instantiate(inAsset.containers[startIndex].triggers[i]);
                g.GetComponent<ITrigger>().StartTrigger();
            }

            if (previewTriggers != null)
            {
                if (previewTriggers.Length - 1 <= startIndex)
                {
                    for (int i = 0; i < previewTriggers[startIndex].triggers.Length; i++)
                    {
                        previewTriggers[startIndex].triggers[i].GetComponent<ITrigger>().StartTrigger();
                    }
                }
            }

        }

        Manager.GetPlayer.disableInput = true;
        Manager.GetPlayer.InDialogue = true;

        StartCoroutine(DelayedStartDialogue(inAsset.containers[startIndex].delay, startIndex, inAsset));
    }

    IEnumerator DelayedStartDialogue(float time, int startIndex, DialogueAsset inAsset)
    {
        dialogueFinished = false;

        yield return new WaitForSecondsRealtime(time);

        portrait.sprite = Resources.Load<Sprite>("Portraits/" + inAsset.containers[startIndex].character.ToString() + "_" + inAsset.containers[startIndex].expression.ToString());
        asset = inAsset;

        OpenDialogue();

        currentDialogueIndex = startIndex;
        content = asset.containers[startIndex].content;

        StartCoroutine(Print());

        currentDialogueIndex++;
    }

    IEnumerator Print()
    {

        int childCount = textPanel.childCount;

        //Clear text panel to prepare for new text
        for (int i = childCount; i > 0; i--)
        {
            Destroy(textPanel.GetChild(i - 1).gameObject);
        }

        int frameCount = 16;
        Sprite actualPortrait = portrait.sprite;

        //Checks if current dialogue is first one or if the character that last spoke is the same one that's speaking now
        if (currentDialogueIndex == 0 || asset.containers[currentDialogueIndex - 1].character != asset.containers[currentDialogueIndex].character)
        {
            for (int i = 0; i < frameCount; i++)
            {
                portrait.sprite = noiseSprites[i % noiseSprites.Length];
                yield return new WaitForSeconds(0.5f/frameCount);
            }
        }

        portrait.sprite = actualPortrait;

        bool popupActive = false;
        bool printItalics = false;
        bool printBold = false;
        int waitMultiplier = 1;
        int audioFrequency = 3;

        string nextWord = "";
        RectTransform rect = textPanel.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = rect.GetComponent<GridLayoutGroup>();

        int xSize = (int)(rect.rect.size.x - gridLayout.padding.left * 2);
        int xSizeCurrent = 0;

        for (int i = 0; i < content.Length; i++)
        {
            //Spawn the letter
            TextMeshProUGUI spawnedLetter = Instantiate(textTemplate, textPanel.transform);
            xSizeCurrent += 25;

            if (content[i] == ' ')
            {
                if (content.IndexOf(" ", i + 1) != -1)
                {
                    nextWord = content.Substring(i + 1, content.IndexOf(" ", i + 1) - i);
                }
                else
                {
                    nextWord = content.Substring(i + 1, content.Length - 1 - i);
                }


                if (xSizeCurrent + nextWord.Length * 25 > xSize)
                {
                    for (int w = 0; w < nextWord.Length; w++)
                    {
                        if (xSizeCurrent + 25 > xSize)
                        {
                            break;
                        }

                        Instantiate(textTemplate, textPanel.transform).text = " ";
                        xSizeCurrent += 25;
                    }

                    xSizeCurrent = 0;
                }
            }

            //Check for special symbols, used for text effects
            switch (content[i])
            {
                case '#':
                    printBold = !printBold;
                    i++;
                    break;
                case '+':
                    printItalics = !printItalics;
                    i++;
                    break;
                case '*':
                    waitMultiplier = int.Parse(content[i + 1].ToString());

                    audioFrequency = Mathf.Clamp(10 - waitMultiplier, 1, 3);

                    i = i + 2;
                    break;
                case '/':
                    popupActive = !popupActive;
                    i++;
                    break;
                default:
                    break;
            }

            //Check if a special letter is last, if so last letter should not be printed
            if (i < content.Length)
            {
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
                if (printItalics)
                {
                    spawnedLetter.fontStyle = FontStyles.Italic;
                }
                if (i % audioFrequency == 0 && content[i] != ' ' && content[i] != '\'')
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Talk" + asset.containers[currentDialogueIndex - 1].character.ToString());
                }
            }

            else
            {
                Destroy(spawnedLetter);
            }

            //Wait between letters happens here
            yield return new WaitForSeconds(delay * waitMultiplier);
        }
        


        dialogueFinished = true;
    }
}
