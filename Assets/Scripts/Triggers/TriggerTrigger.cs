using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrigger : MonoBehaviour, ITrigger {

    public GameObject[] triggers;
    private bool actuallyTriggered;

    void OnValidate()
    {
        if (!GetComponent<BoxCollider2D>())
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        if (!GetComponent<Util_DrawTrigger>())
        {
            gameObject.AddComponent<Util_DrawTrigger>();
        }
    }

    public void OnEventFinished()
    {

    }

    public void StartTrigger()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].GetComponent<ITrigger>().StartTrigger();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !actuallyTriggered)
        {
            actuallyTriggered = true;
            StartTrigger();
        }
    }
}
