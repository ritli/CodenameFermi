using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

    public Animator menuAnimator, optionsAnimator, helpAnimator, creditsAnimator;

    bool menuOpen = false, optionsOpen = false, creditsOpen = false;
    private bool quittingGame;

    Button[] buttons;

    public void Init()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void ToggleCredits()
    {
        if (creditsOpen)
        {
            CloseCredits();
        }
        else
        {
            OpenCredits();
        }
    }

    void CloseCredits()
    {

        creditsAnimator.Play("Close");
        creditsOpen = false;
    }

    void OpenCredits()
    {
        creditsAnimator.Play("Open");
        creditsOpen = true;
    }

    public void CloseMenu()
    {
        if (menuOpen)
        {
            CloseOptions();
            menuAnimator.Play("Close");
            menuOpen = false;

            helpAnimator.Play("Open");

            foreach (var item in optionsAnimator.GetComponentsInChildren<Slider>())
            {
                item.interactable = false;
            }

            foreach (Button b in buttons)
            {
                b.GetComponent<UI_HoverSelectPlayer>().playSound = false;


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

        foreach (var item in optionsAnimator.GetComponentsInChildren<Slider>())
        {
            item.interactable = false;
        }

        foreach (Button b in buttons)
        {
            b.GetComponent<UI_HoverSelectPlayer>().playSound = false;

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
                b.GetComponent<UI_HoverSelectPlayer>().playSound = true;

                b.interactable = true;
            }
        }
    }

    public void ToggleOptions()
    {
        if (optionsOpen)
        {
            if (creditsOpen)
            {
                CloseCredits();
            }

            optionsAnimator.Play("Close");

            foreach (var item in optionsAnimator.GetComponentsInChildren<Slider>())
            {
                item.interactable = false;
            }

            foreach (var item in optionsAnimator.GetComponentsInChildren<Button>())
            {
                item.interactable = false;
            }


        }
        else
        {
            optionsAnimator.Play("Open");

            foreach (var item in optionsAnimator.GetComponentsInChildren<Slider>())
            {
                item.interactable = true;
            }

            foreach (var item in optionsAnimator.GetComponentsInChildren<Button>())
            {
                item.interactable = true;
            }
        }

        optionsOpen = !optionsOpen;
    }

    void CloseOptions()
    {
        if (optionsOpen)
        {
            if (creditsOpen)
            {
                CloseCredits();
            }

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
