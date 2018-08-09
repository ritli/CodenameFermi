using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMusicTrackTrigger : MonoBehaviour, ITrigger {

    public bool startEvent, stopEvent, useParameter, destroyTrack;
    public string parameter;
    public float parameterValue;

    public void OnEventFinished()
    {

    }

    public void StartTrigger()
    {
        if (!destroyTrack)
        {
            if (startEvent)
            {
                ActiveMusicTrack.StartEvent();
            }
            if (stopEvent)
            {
                ActiveMusicTrack.StopEvent();
            }
            if (useParameter)
            {
                ActiveMusicTrack.SetParameter(parameter, parameterValue);
            }
        }
        else
        {
            ActiveMusicTrack.DestroyMusicTrack();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            StartTrigger();
        }
    }

}
