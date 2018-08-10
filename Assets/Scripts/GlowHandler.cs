using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHandler : MonoBehaviour {

    public bool glowOnce = true;

    public float lifeTime = 0.5f;
    private float timeElapsed;

    void Start () {
        print(GetComponentInParent<SpriteRenderer>().sprite.name);

        GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder - 1;

        Animator animator = GetComponent<Animator>();
        animator.enabled = true;

        if (glowOnce)
        {
            animator.Play("GlowOnce");
        }
        else
        {
            animator.Play("Glow");
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
