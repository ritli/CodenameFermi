using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour {

    public float requiredTime = 1f;
    public int requiredHits = 4;
    float lastTime = 0;
    private Animator animator;

    float cooldown = 0.25f, cooldownElapsed = 0;

    int hits = 0;

    void Start () {
        animator = GetComponent<Animator>();
	}

    private void Update()
    {
        cooldownElapsed += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gun") && cooldownElapsed > cooldown)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/HitMetal");

            if (Time.time - lastTime < requiredTime)
            {
                hits++;

                if (hits > requiredHits)
                {
                    GetComponent<Collider2D>().enabled = false;
                    animator.Play("Open");
                }
            }
            else
            {
                hits = 0;
            }

            lastTime = Time.time;
        }
    }

    public void DestroyDoor()
    {
        animator.Play("InstaOpen");
        GetComponent<Collider2D>().enabled = false;
        Instantiate(Resources.Load<GameObject>("DoorDestroy"), transform.position, Quaternion.identity);
    }
}
