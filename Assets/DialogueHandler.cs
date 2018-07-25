using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour {

    [Multiline]
    public string content;
    public TextMeshProUGUI textTemplate;
    public Transform textPanel;
    public Image portrait;

    public float delay = 0.02f;

    void Start () {
        StartCoroutine(Print());
	}
	
    IEnumerator Print()
    {
        int childCount = textPanel.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(textPanel.GetChild(0));
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
    }
}
