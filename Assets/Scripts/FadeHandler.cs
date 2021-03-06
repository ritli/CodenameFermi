﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeHandler : MonoBehaviour {

    Image image;

    bool isInit = false;

	void Start () {
        Init();
	}
	
    void Init()
    {
        if (isInit)
        {
            return;
        }

        image = GetComponent<Image>();
    }

    public void FadeOut(float time, Color color)
    {
        print("Fading out");

        Init();

        color.a = image.color.a;
        image.color = color;

        StartCoroutine(FadeRoutine(time, 1));
    }

    public void FadeIn(float time, Color color)
    {
        print("Fading in");

        Init();

        color.a = image.color.a;
        image.color = color;

        StartCoroutine(FadeRoutine(time, 0));
    }

    IEnumerator FadeRoutine(float time, float toAlpha)
    {
        float timeElapsed = 0;
        Color c = image.color;

        while (timeElapsed < time)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            c.a = Mathf.Lerp(c.a, toAlpha, timeElapsed / time);
            image.color = c;
        }
    }

    public void ShowCredits()
    {
        transform.Find("EndCredits").gameObject.SetActive(true);

        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(27f);

        SceneManager.LoadScene("1_Ship");
    }
}
