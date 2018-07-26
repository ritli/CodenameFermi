using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public DialogueAsset dialogueAsset;

    new private BoxCollider2D collider;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Manager.GetDialogue.StartDialogue(0, dialogueAsset);
            GetComponent<Collider2D>().enabled = false;
        }
    }


}
