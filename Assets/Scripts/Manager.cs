using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public static Manager instance;

    Player player;
    new CameraController camera;
    private DialogueHandler dialogue;
    private FadeHandler fadeHandler;

    //Special object only for dialogue at scene start
    private DialogueTriggerSceneStart startDialogue;

    void Awake()
    {
        if (FindObjectsOfType<Manager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Init();

            instance = this;
        }
    }

    /// <summary>
    /// Finds and sets all Manager variables, 
    /// DOES NOT INIT THE VARIABLES THEMSELVES SO DONT RUN START() DEPENDENT CODE HERE
    /// </summary>
    void Init()
    {
        camera = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        dialogue = GetComponentInChildren<DialogueHandler>(true);
        fadeHandler = GetComponentInChildren<FadeHandler>(true);
        fadeHandler.gameObject.SetActive(true);

        startDialogue = FindObjectOfType<DialogueTriggerSceneStart>();
    }

    void Start() {
        StartCoroutine(InitRoutine());
    }

    public void StartScene()
    {
        fadeHandler.FadeIn(1f, Color.white);
    }

    /// <summary>
    /// A routine run at scene start after all objects have been initialized
    /// </summary>
    IEnumerator InitRoutine()
    {
        yield return new WaitForSeconds(0.25f);

        if (startDialogue)
        {
            startDialogue.StartTrigger();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartScene();
        }
    }

    public static Player GetPlayer{
        get
        {
            return instance.player;
        }
    }

    public static CameraController GetCamera
    {
        get
        {
            return instance.camera;
        }
    }

    public static DialogueHandler GetDialogue
    {
        get
        {
            return instance.dialogue;
        }
    }

    public static FadeHandler GetFade
    {
        get
        {
            return instance.fadeHandler;
        }
    }
}
