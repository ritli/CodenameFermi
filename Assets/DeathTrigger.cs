using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

    public DialogueAsset dialogueAsset;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.GetDialogue.StartDialogue(0, dialogueAsset);
        }
    }
}
