using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITrigger
{
    void StartTrigger();
    void OnEventFinished();
}

public class DialogueTrigger : MonoBehaviour, ITrigger {

    public DialogueAsset dialogueAsset;
    new private BoxCollider2D collider;

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
        Manager.GetDialogue.StartDialogue(0, dialogueAsset);
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
