using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed, jumpForce;

    new Rigidbody2D rigidbody;
    Vector2 input;
    public LayerMask layermask;

    Vector2 feetOrigin = new Vector2(0.25f, -0.5f);
    private bool isJumping, inAir = false;
    const float feetLength = 0.05f;


    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        InputUpdate();
        MovementUpdate();
        JumpUpdate();

        inAir = !OnGround;
	}

    [Range(0, 0.3f)]
    public float xSnappiness = 0.05f;
    [Range(0, 0.5f)]
    [Tooltip("This is added to normal snappiness while in air")]
    public float xAirSnappiness = 0.2f;

    float xVel, xVelCurrent = 0, xDecay; 

    private void MovementUpdate()
    {
        xDecay = 0;
        if (inAir)
        {
            xDecay = xAirSnappiness;
        }

        xVel = Mathf.SmoothDamp(xVel, input.x * speed, ref xVelCurrent, xSnappiness + xDecay);

        transform.Translate(Vector3.right * xVel * Time.deltaTime);
    }

    void InputUpdate()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && OnGround){
            Jump();
        }
    }

    public float jumptimeMax = 0.6f, gravity = -1; 
    float jumptime = 0, jumpVelocity = 0; 

    void JumpUpdate()
    {
        if (isJumping && jumptime < jumptimeMax)
        {
            transform.Translate(Vector2.up * (jumpVelocity * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime));
            jumpVelocity += gravity * Time.deltaTime;

            jumptime += Time.deltaTime;
        }
        else
        {
            if (inAir)
            {
                transform.Translate(Vector2.up * (jumpVelocity * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime));
                jumpVelocity += gravity * Time.deltaTime;
            }
            else
            {
                jumpVelocity = 0;
            }

            isJumping = false;
        }
    }

    private void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            jumpVelocity = jumpForce;
            jumptime = 0;
        }
    }

    bool OnGround
    {
        get
        {
            Vector2 origin = feetOrigin;

            if (Physics2D.Linecast((Vector2)transform.position + feetOrigin, (Vector2)transform.position + feetOrigin + Vector2.down * feetLength, layermask))
            {
                return true;
            }

            else
            {
                origin.x = -origin.x;
                if (Physics2D.Linecast((Vector2)transform.position + feetOrigin, (Vector2)transform.position + feetOrigin + Vector2.down * feetLength, layermask))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
