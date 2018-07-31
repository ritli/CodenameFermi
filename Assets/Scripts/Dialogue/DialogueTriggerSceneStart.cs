using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerSceneStart : MonoBehaviour, ITrigger {
    [Header("Place this in the scene if you want to play dialogue before fade in.")]
    [Space]
    public DialogueAsset dialogueAsset;

    public void OnEventFinished()
    {
        Manager.instance.StartScene();
    }

    public void StartTrigger()
    {
        Manager.GetDialogue.trigger = gameObject;
        Manager.GetDialogue.StartDialogue(0, dialogueAsset);
    }
}
