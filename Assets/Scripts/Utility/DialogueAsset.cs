using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Character
{
    Velvet, Player, AI
}

public enum Expression
{
    Neutral, Angry, Happy, Surprised
}

[System.Serializable]
public struct DialogueContainer
{
    public Character character;
    public Expression expression;
    [TextArea(5, 10)]
    public string content;
    public bool endDialogue;
    public GameObject[] triggers;
    public bool playTriggerAtEndInstead;
    public float delay;
    public bool showStatic;
    public bool skipConfirmation;
}



public class DialogueAsset : ScriptableObject {

    [Header("# = Bold")]
    [Header("+ = Italics")]
    [Header("*NUMBER = Set Text Speed")]
    [Header("/ = Popup")]

    public DialogueContainer[] containers;

#if UNITY_EDITOR

    [MenuItem("Assets/Create/DialogueAsset")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<DialogueAsset>();
    }
#endif
}
