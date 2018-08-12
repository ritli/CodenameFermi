using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookTrigger : MonoBehaviour, ITrigger {

    public bool triggerOnce;
    bool triggered;
    public string triggerTagFilter = "Player";
    public CameraLookSettings settings;

    Transform lookPos;
#if UNITY_EDITOR
    void OnValidate()
    {
        if (!transform.Find("Look Position") && !lookPos)
        {
            lookPos = new GameObject("Destination").transform;
            lookPos.transform.parent = transform;
            lookPos.localPosition = Vector3.zero;
        }

        if (lookPos)
        {
            settings.lookPosition = lookPos.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (lookPos)
        {
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            Gizmos.DrawSphere(lookPos.position, 1);
        }
    }
#endif
    public void OnEventFinished()
    {
        throw new System.NotImplementedException();
    }

    public void StartTrigger()
    {
        print("Triggered");

        if (triggerOnce)
        {
            Manager.GetCamera.TimedLook(settings);
            triggered = true;
        }

        else
        {
            Manager.GetCamera.TimedLookToggle(true, settings);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(triggerTagFilter))
        {
            StartTrigger();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(triggerTagFilter))
        {
            Manager.GetCamera.TimedLookToggle(false, settings);
        }
    }
}
