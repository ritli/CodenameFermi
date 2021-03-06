﻿using System.Collections;
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
    public bool inMenu, startFromMenu, fadeIn;
    private bool gamePaused;

    float playerGravity = 1;

    FMOD.Studio.EventInstance inMenuPauseInstance;

    [Header("AUDIO")]
    public FMOD.Studio.Bus musicBus;
    public FMOD.Studio.Bus soundBus;

    FMODUnity.StudioEventEmitter mainthemeEmitter;

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
        GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);

        camera = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        dialogue = GetComponentInChildren<DialogueHandler>(true);
        fadeHandler = GetComponentInChildren<FadeHandler>(true);
        fadeHandler.gameObject.SetActive(true);
        menuHandler = GetComponentInChildren<MenuHandler>(true);
        menuHandler.Init();
        menuHandler.InstaClose();
        inMenu = false;
        startDialogue = FindObjectOfType<DialogueTriggerSceneStart>();

        player.disableInput = true;

        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        soundBus = FMODUnity.RuntimeManager.GetBus("bus:/Sound");

        inMenuPauseInstance = FMODUnity.RuntimeManager.CreateInstance("snapshot:/InMenu");
    }

    void Start() {
        StartCoroutine(InitRoutine());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Escape") && !startFromMenu)
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
        inMenuPauseInstance.start();

        inMenu = true;
        menuHandler.OpenMenu();
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        inMenuPauseInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        inMenu = false;
        menuHandler.CloseMenu();
        Time.timeScale = 1;

        if (startFromMenu)
        {
            startFromMenu = false;
            StartCoroutine(InitRoutine());
        }
    }

    public void StartScene()
    {
        player.disableInput = false;
        camera.TimedLookToggle(false, player.transform.position);

        if (mainthemeEmitter)
        {
            mainthemeEmitter.Stop();
            Destroy(mainthemeEmitter);
        } 

        if (playerGravity != 1)
        {
            player.gravity = playerGravity;
        }

        fadeHandler.FadeIn(5f, Color.white);
    }

    /// <summary>
    /// A routine run at scene start after all objects have been initialized
    /// </summary>
    IEnumerator InitRoutine()
    {
        yield return new WaitForSeconds(0.01f);

        MusicVolume = MusicVolume;
        AudioVolume = AudioVolume;

        if (!startFromMenu)
        {
            if (startDialogue)
            {
                playerGravity = player.gravity;
                player.gravity = 0;

                camera.TimedLookToggle(false, Vector2.zero);
                startDialogue.StartTrigger();

                if (mainthemeEmitter)
                {
                    mainthemeEmitter.Stop();
                    Destroy(mainthemeEmitter);
                }
            }
            else
            {
                print("Starting Scene from Init");
                yield return new WaitForSeconds(0.1f);
                StartScene();

                yield return new WaitForSeconds(0.1f);

                camera.m_interpolateCamera = false;

                yield return new WaitForSeconds(0.1f);
                camera.m_interpolateCamera = true;

            }
        }

        else
        {


            if (fadeIn)
            {
                fadeHandler.FadeIn(1f, Color.white);
            }
            menuHandler.OpenMenu();
            CameraLookSettings settings = new CameraLookSettings();
            settings.lookPosition = transform.position;
            settings.zoomLevel = 4;
            settings.addZoomLevelToCurrentZoom = true;

            camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
            Time.timeScale = 0;

            camera.TimedLookToggle(true, settings);

            mainthemeEmitter = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
            mainthemeEmitter.Event = "event:/MusicMain";
            mainthemeEmitter.Play();
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

    public static float AudioVolume
    {
        set
        {
            float volume = value;

            PlayerPrefs.SetFloat("AudioVolume", volume);
            PlayerPrefs.Save();

            instance.soundBus.setVolume(volume);
        }

        get
        {
            if (PlayerPrefs.HasKey("AudioVolume"))
            {
                return PlayerPrefs.GetFloat("AudioVolume");
            }
            else
            {
                float vol, finalVol;
                instance.soundBus.getVolume(out vol, out finalVol);

                AudioVolume = vol;

                return vol;
            }
        }
    }

    public static float MusicVolume
    {
        set
        {
            float volume = value;

            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();

            instance.musicBus.setVolume(volume);
        }

        get
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                return PlayerPrefs.GetFloat("MusicVolume");
            }
            else
            {
                float vol, finalVol;
                instance.musicBus.getVolume(out vol, out finalVol);

                AudioVolume = vol;
                return vol;
            }
        }
    }

    public static void OpenStarmap()
    {
        instance.GetComponentInChildren<StarmapHandler>().OpenStarmap();
    }
}
