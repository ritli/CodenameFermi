using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

    public Animator menuAnimator, optionsAnimator, helpAnimator;

    bool menuOpen = false, optionsOpen = false;
    private bool quittingGame;

    Button[] buttons;

    public void Init()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void CloseMenu()
    {
        if (menuOpen)
        {
            CloseOptions();
            menuAnimator.Play("Close");
            menuOpen = false;

            helpAnimator.Play("Open");

            foreach (Button b in buttons)
            {
                b.interactable = false;
            }
        }
    }

    public void InstaClose()
    {
        CloseOptions();

        helpAnimator.Play("Open");

        menuAnimator.Play("InstaClose");
        menuOpen = false;

        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
    }


    public void OpenMenu()
    {
        if (!menuOpen)
        {
            menuAnimator.Play("Open");
            menuOpen = true;

            helpAnimator.Play("Close");

            foreach (Button b in buttons)
            {
                b.interactable = true;
            }
        }
    }

    public void ToggleOptions()
    {
        if (optionsOpen)
        {
            optionsAnimator.Play("Close");
        }
        else
        {
            optionsAnimator.Play("Open");
        }

        optionsOpen = !optionsOpen;
    }

    void CloseOptions()
    {
        if (optionsOpen)
        {
            optionsOpen = false;
            optionsAnimator.Play("Close");
        }
    }

    public void ExitGame()
    {
        if (!quittingGame)
        {
            quittingGame = true;

            StartCoroutine(ExitGameRoutine());
        }
    }

    IEnumerator ExitGameRoutine()
    {
        menuAnimator.Play("Close");
        Manager.GetFade.FadeOut(1, Color.black);

        yield return new WaitForSecondsRealtime(1.5f);

        Application.Quit();
    }

    public void OnSoundSliderChanged(float val)
    {
        Manager.AudioVolume = val;
    }

    public void OnMusicSliderChanged(float val)
    {
        Manager.MusicVolume = val;
    }
}
