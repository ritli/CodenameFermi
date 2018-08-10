using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatformHandler : MonoBehaviour {
    private Animator animator;

    public float edgeThreshhold = 1;

    float cooldownTimestamp = 0;

    void Start () {
        animator = GetComponent<Animator>();
	}
	
	void Update () {
		
	}
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + Vector3.right * edgeThreshhold, transform.position + Vector3.right * edgeThreshhold + Vector3.up);
        Gizmos.DrawLine(transform.position + Vector3.right * -edgeThreshhold, transform.position + Vector3.right * -edgeThreshhold + Vector3.up);
    }
#endif
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - cooldownTimestamp > 1)
        {
            if (collision.CompareTag("Player"))
            {
                Vector2 pos = collision.transform.position;

                if (pos.x - transform.position.x > edgeThreshhold)
                {
                    animator.Play("SwayRight");
                }
                else if (pos.x - transform.position.x < -edgeThreshhold)
                {
                    animator.Play("SwayLeft");
                }
                else
                {
                    animator.Play("Sway");
                }

                Manager.GetPlayer.transform.parent = transform.GetChild(0);
            }

            cooldownTimestamp = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 pos = collision.transform.position;

            if (pos.x - transform.position.x > edgeThreshhold)
            {
                animator.Play("SwayRight");
            }
            else if (pos.x - transform.position.x < -edgeThreshhold)
            {
                animator.Play("SwayLeft");
            }
            else
            {
                animator.Play("Sway");
            }

            Manager.GetPlayer.transform.parent = null;
        }
    }
}
