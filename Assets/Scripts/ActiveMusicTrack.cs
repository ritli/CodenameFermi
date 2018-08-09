using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMusicTrack : MonoBehaviour {

    [FMODUnity.EventRef]
    public string eventRef;
    public FMOD.Studio.EventInstance eventInstance;
    public FMODUnity.StudioEventEmitter emitter;

    public static ActiveMusicTrack instance;

	void Start () {
		if (FindObjectsOfType<ActiveMusicTrack>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        }
	}

    public static void StartEvent()
    {
        instance.emitter.Event = instance.eventRef;
        instance.emitter.Play();
        instance.eventInstance = instance.emitter.EventInstance;
    }

    public static void SetParameter(string parameter, float value)
    {
        instance.eventInstance.setParameterValue(parameter, value);
    }

    public static void StopEvent()
    {
        instance.emitter.Stop();
    }

    public static void DestroyMusicTrack()
    {
        StopEvent();
        Destroy(instance.gameObject);
    }
}
