using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_HoverSelectPlayer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {

    [FMODUnity.EventRef]
    public string hover;
    [FMODUnity.EventRef]
    public string click;

    public bool playSound = true;

    float time = 0, clickTime = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.unscaledTime - clickTime > 0.5f && playSound)
        {
            FMODUnity.RuntimeManager.PlayOneShot(click);
            clickTime = Time.unscaledTime;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Time.unscaledTime - time > 0.5f && playSound)
        {
            time = Time.unscaledTime;
            FMODUnity.RuntimeManager.PlayOneShot(hover);
        }
    }
}
