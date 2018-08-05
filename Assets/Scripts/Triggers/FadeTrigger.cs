using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrigger : MonoBehaviour, ITrigger {

    public float time;
    public bool fadeIn;
    public Color color = Color.white;

    public void OnEventFinished()
    {
    }

    public void StartTrigger()
    {
        if (fadeIn)
        {
            Manager.GetFade.FadeIn(time, color);
        }
        else
        {
            Manager.GetFade.FadeOut(time, color);
        }
    }
}
