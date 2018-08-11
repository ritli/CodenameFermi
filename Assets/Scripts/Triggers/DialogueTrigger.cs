using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITrigger
{
    void StartTrigger();
    void OnEventFinished();
}

[System.Serializable]
public struct DialoguePreview
{
    public string name;
    public GameObject[] triggers;
}

public class DialogueTrigger : MonoBehaviour, ITrigger {

    public DialogueAsset dialogueAsset;
    new private BoxCollider2D collider;

    public DialoguePreview[] previews;

    [ContextMenu("Refresh Previews")]
    void RefreshPreviews()
    {
        if (dialogueAsset != null)
        {
            DialoguePreview[] oldPreviews = previews;

            previews = new DialoguePreview[dialogueAsset.containers.Length];

            for (int i = 0; i < dialogueAsset.containers.Length; i++)
            {
                previews[i].name = dialogueAsset.containers[i].character.ToString() + " - " + dialogueAsset.containers[i].content.Substring(0, Mathf.Clamp(50, 0, dialogueAsset.containers[i].content.Length));

                if (i <= oldPreviews.Length - 1)
                {
                    previews[i].triggers = new GameObject[oldPreviews[i].triggers.Length];

                    for (int p = 0; p < oldPreviews[i].triggers.Length; p++)
                    {
                        previews[i].triggers[p] = oldPreviews[i].triggers[p];
                    }
                }
            }
        }
    }

    void OnValidate()
    {
        RefreshPreviews();
    }

    public void OnEventFinished()
    {
        if (GetComponent<SceneTransition>())
        {
            GetComponent<SceneTransition>().ChangeScene();
        }
    }

    public void StartTrigger()
    {
        Manager.GetDialogue.trigger = gameObject;
        Manager.GetDialogue.StartDialogue(0, dialogueAsset, previews);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            StartTrigger();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
