using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public static Manager instance;

    Player player;
    new CameraController camera;
    private DialogueHandler dialogue;
    private FadeHandler fadeHandler;
    private MenuHandler menuHandler;

    //Special object only for dialogue at scene start
    private DialogueTriggerSceneStart startDialogue;
    public bool inMenu, startFromMenu;
    private bool gamePaused;


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
        menuHandler = GetComponentInChildren<MenuHandler>(true);
        startDialogue = FindObjectOfType<DialogueTriggerSceneStart>();

        player.disableInput = true;
    }

    void Start() {
        StartCoroutine(InitRoutine());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            gamePaused = !gamePaused;

            if (!inMenu)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    public void Pause()
    {
        inMenu = true;
        menuHandler.OpenMenu();
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        inMenu = false;
        menuHandler.CloseMenu();
        Time.timeScale = 1;
    }

    public void StartScene()
    {
        player.disableInput = false;
        camera.TimedLookToggle(false, Vector2.zero);

        fadeHandler.FadeIn(1f, Color.white);
    }

    /// <summary>
    /// A routine run at scene start after all objects have been initialized
    /// </summary>
    IEnumerator InitRoutine()
    {
        yield return new WaitForSeconds(0.25f);

        if (!startFromMenu)
        {

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

        else
        {
            fadeHandler.FadeIn(1f, Color.white);
            menuHandler.OpenMenu();
            CameraLookSettings settings = new CameraLookSettings();
            settings.lookPosition = camera.transform.position;
            settings.zoomLevel = 2;
            settings.addZoomLevelToCurrentZoom = true;

            camera.TimedLookToggle(true, settings);
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
