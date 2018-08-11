using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTriggersAfterTime : MonoBehaviour, ITrigger {

    public float time = 5f;

    float elapsedTime;

    public bool playerInArea = false;
    bool triggered = false;
    public GameObject[] triggers; 

	void Update () {
        if (elapsedTime > time)
        {
            StartTrigger();
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = false;
        }
    }

    public void StartTrigger()
    {
        if (!triggered)
        {
            triggered = true;

            for (int i = 0; i < triggers.Length; i++)
            {
                triggers[i].GetComponent<ITrigger>().StartTrigger();
            }
        }
    }

    public void OnEventFinished()
    {
    }
}
