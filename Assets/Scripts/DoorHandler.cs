using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour {

    public float requiredTime = 1f;
    public int requiredHits = 4;
    float lastTime = 0;
    private Animator animator;
    private Transform player;
    float cooldown = 0.5f, cooldownElapsed = 0;

    public float musicDampenRange = 10;

    int hits = 0;

    void Start () {
        animator = GetComponent<Animator>();

        player = Manager.GetPlayer.transform;
	}

    private void Update()
    {
        if (Vector2.Distance(player.position, transform.position) < musicDampenRange)
        {
			if (ActiveMusicTrack.instance)
			{
				ActiveMusicTrack.SetParameter("Dark", ((1 - (Vector2.Distance(player.position, transform.position) / musicDampenRange)) * 100));
			}
		}


        cooldownElapsed += Time.deltaTime;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, musicDampenRange);
    }
#endif
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gun") && cooldownElapsed > cooldown && Mathf.Abs(Manager.GetPlayer.Velocity.x) < 0.1f)
        {
			print("MEME");

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
        FMODUnity.RuntimeManager.PlayOneShot("event:/LargeDoorDestroy");

        animator.Play("InstaOpen");
        GetComponent<Collider2D>().enabled = false;
        Instantiate(Resources.Load<GameObject>("DoorDestroy"), transform.position, Quaternion.identity);
    }
}
