using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAudioPlayer : MonoBehaviour {

    string[] eventNames;

    void PlayRandomClipPrivate(string name)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/" + name, gameObject); 
    }


}
