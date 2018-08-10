using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour, ITrigger {

    public float time, amount;

    public void OnEventFinished()
    {

    }

    public void StartTrigger()
    {
        Manager.GetCamera.SetScreenShake(time, amount);
    }
}
