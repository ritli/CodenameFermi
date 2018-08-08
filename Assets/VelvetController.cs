using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelvetController : MonoBehaviour {

    public bool isDead;

    Animator animator;
    private Transform player;

    void Start () {
        animator = GetComponent<Animator>();
        player = Manager.GetPlayer.transform;
    }
	
	void Update () {

        if (isDead)
        {
            animator.Play("Dead");
        }
        else
        {
            if (player.position.x > transform.position.x)
            {
                animator.Play("IdleRight");
            }
            else
            {
                animator.Play("IdleLeft");
            }
        }

    }
}
