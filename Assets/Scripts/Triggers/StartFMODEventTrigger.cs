using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFMODEventTrigger : MonoBehaviour, ITrigger {

    [FMODUnity.EventRef]
    public string eventRef;

    public void OnEventFinished()
    {

    }

    public void StartTrigger()
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventRef);
    }
}
