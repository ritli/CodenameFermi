using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHandler : MonoBehaviour {

    public bool glowOnce = true;

    public float lifeTime = 0.5f;
    private float timeElapsed;

    void Start () {
        GetComponent<SpriteRenderer>().sprite = GetComponentInParent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder - 1;


        if (glowOnce)
        {

            GetComponent<Animator>().Play("GlowOnce");
        }
        else
        {
            GetComponent<Animator>().Play("Glow");

        }
    }
	
	void Update () {

        if (glowOnce)
        {
            if (timeElapsed > lifeTime)
            {
                Destroy(gameObject);
            }

            timeElapsed += Time.deltaTime;
        }
	}
}
