using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public LayerMask obstacleLayermask;

    [Header("Ground Parameters")]
    public float speed;
    [Range(0.3f, 0)]
    public float snappiness = 0.05f;
    [Header("Air Parameters")]
    public float jumpForce;
    [Range(0.5f, 0)]
    [Tooltip("This is added to normal snappiness while in air, 0 is full control while in air")]
    public float airControl = 0.2f;
    public float jumptimeMax = 0.6f, gravity = -1;

    new Rigidbody2D rigidbody;

    Vector2 feetOrigin = new Vector2(0.25f, -0.5f);
    Vector2 input;

    private bool isJumping, inAir = false;

    float xVel, xVelCurrent = 0, airSnappiness;
    float jumptime = 0, jumpVelocity = 0;

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

    private void MovementUpdate()
    {
        airSnappiness = 0;

        if (inAir)
        {
            airSnappiness = airControl;

            //In case of full air control, snappiness is completely negated
            if (airControl == 0)
            {
                airSnappiness = -snappiness;
            }
        }

        //Actual velocity is calculated here
        xVel = Mathf.SmoothDamp(xVel, input.x * speed, ref xVelCurrent, snappiness + airSnappiness);

        //Translation is multiplied by this
        float collisionMultiplier = 1;

        //Checks for collisions, if side collsion occurs, player movement is halted
        if (xVel != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                float xVelSigned = Mathf.Clamp(xVel, -1, 1);
                Vector2 origin = (Vector2)transform.position + new Vector2(feetOrigin.x * xVelSigned, Mathf.Lerp(feetOrigin.y + 0.1f, -feetOrigin.y - 0.1f, i / 2f));

                if (Physics2D.Linecast(origin, origin + Vector2.right * xVelSigned * feetLength * 2, obstacleLayermask))
                {
                    collisionMultiplier = 0f;
                    break;
                }
            }
        }

        //Movement is applied
        transform.Translate(Vector3.right * xVel * Time.deltaTime * collisionMultiplier);
    }

    void InputUpdate()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && OnGround){
            Jump();
        }
    }


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

            if (Physics2D.Linecast((Vector2)transform.position + feetOrigin, (Vector2)transform.position + feetOrigin + Vector2.down * feetLength, obstacleLayermask))
            {
                return true;
            }

            else
            {
                origin.x = -origin.x;
                if (Physics2D.Linecast((Vector2)transform.position + feetOrigin, (Vector2)transform.position + feetOrigin + Vector2.down * feetLength, obstacleLayermask))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
