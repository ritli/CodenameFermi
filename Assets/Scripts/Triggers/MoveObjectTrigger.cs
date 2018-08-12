using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MoveObjectTrigger : MonoBehaviour, ITrigger
{
    public float time;

    public Transform objectToMove;
    public Transform destination;

#if UNITY_EDITOR

    void OnValidate()
    {
        if (!transform.Find("Destintation") && !destination)
        {
            destination = new GameObject("Destination").transform;
            destination.transform.parent = transform;
        }
    }

    void OnDrawGizmos()
    {
        if (objectToMove && destination)
        {
            Debug.DrawLine(objectToMove.position, destination.position);
        }
    }

#endif

    public void OnEventFinished()
    {
        throw new System.NotImplementedException();
    }

    public void StartTrigger()
    {
        objectToMove.DOMove(destination.position, time);
    }
}
