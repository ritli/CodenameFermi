using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public static Manager instance;

    Player player;
    new CameraController camera;

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

    void Init()
    {
        camera = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
    }

    void Start() {

    }

    void Update() {

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
}
