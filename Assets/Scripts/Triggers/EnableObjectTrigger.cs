using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectTrigger : MonoBehaviour, ITrigger {

    public bool enableObjects;

    public GameObject[] objects;

    public void OnEventFinished()
    {
    }

    public void StartTrigger()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(enableObjects);
        }
    }
}
