using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Character
{
    Velvet, Player, AI
}

[System.Serializable]
public struct DialogueContainer
{
    public Character character;
    [TextArea(5, 10)]
    public string content;
    public bool continueDialogue;
}

public class DialogueAsset : ScriptableObject {

    public DialogueContainer[] containers;

    [MenuItem("Assets/Create/DialogueAsset")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<DialogueAsset>();
    }
}
