using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour {

    public float requiredTime = 1f;
    public int requiredHits = 4;
    float lastTime = 0;
    private Animator animator;

    float cooldown = 0.5f, cooldownElapsed = 0;

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
        if (other.CompareTag("Gun") && cooldownElapsed > cooldown && Mathf.Abs(Manager.GetPlayer.Velocity.x) < 0.1f)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/HitMetal");



            if (Time.time - lastTime < requiredTime)
            {
                hits++;

                if (hits > requiredHits)
                {
                    Manager.GetCamera.SetScreenShake(2.6f, 0.2f);

                    GetComponent<Collider2D>().enabled = false;
                    animator.Play("Open");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/LargeDoorOpen");

                }
            }
            else
            {
                hits = 0;
            }

            lastTime = Time.time;
        }

        if (other.CompareTag("Magnetic"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/HitMetal");
                
            Manager.GetCamera.SetScreenShake(2.6f, 0.2f);

            GetComponent<Collider2D>().enabled = false;
            animator.Play("Open");
            FMODUnity.RuntimeManager.PlayOneShot("event:/LargeDoorOpen");
        }
    }

    public void DestroyDoor()
    {
        animator.Play("InstaOpen");
        GetComponent<Collider2D>().enabled = false;
        Instantiate(Resources.Load<GameObject>("DoorDestroy"), transform.position, Quaternion.identity);
    }
}
