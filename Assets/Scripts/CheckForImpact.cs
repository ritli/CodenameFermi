using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForImpact : MonoBehaviour {

    float delay = 1;
    const float maxDelay = 1;
    
    [FMODUnity.EventRef]
    public string fmodEvent;

	void Update () {
        delay = Mathf.Clamp(delay - Time.deltaTime, 0, maxDelay);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (delay <= 0 && !collision.collider.CompareTag("Player"))
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(fmodEvent, gameObject);
            delay = maxDelay;
        }    
    
    }
}
