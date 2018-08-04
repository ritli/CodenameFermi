using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    public Animator menuAnimator;

    bool menuOpen = true;

    public void CloseMenu()
    {
        if (menuOpen)
        {
            menuAnimator.Play("Close");
            Manager.instance.StartScene();
            menuOpen = false;
        }
    }

    public void OpenMenu()
    {
        if (!menuOpen)
        {
            menuAnimator.Play("Open");
            menuOpen = true;
        }
    }
}
