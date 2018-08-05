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

    [Header("Control Parameters")]
    public bool disableInput = false;

    private Animator animator;
    AudioClipPlayer animatorAudio;
    new Rigidbody2D rigidbody;
    private SpriteRenderer sprite;
    private SpriteRenderer gunSprite;
    private Camera cam;

    Vector2 feetOrigin = new Vector2(0.25f, -0.5f);
    Vector2 input;

    private bool inAirOld;
    private bool isJumping;
    [HideInInspector]
    public bool inAir = false;

    float xVel, xVelCurrent = 0, airSnappiness;
    float jumptime = 0, jumpVelocity = 0;

    private float addForceSnappiness;
    private Vector3 lookPos;
    const float feetLength = 0.05f;

    [HideInInspector]
    public bool facingRight, moving;
    private float autoRunTime;
    private Vector2 autoRunVector;

    MultiParticleHandler trailParticles;
    private bool trailActive = true;
    bool inDialogue = false;

    void Start ()
    {
        trailParticles = transform.Find("Trail").GetComponent<MultiParticleHandler>();
        animator = transform.Find("Sprite").GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        gunSprite = sprite.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        cam = Camera.main;
        animatorAudio = animator.GetComponent<AudioClipPlayer>();

        if (trailActive)
        {
            SetTrailActive(false);
        }
    }
	
	void Update () {
        lookPos = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        InputUpdate();
        MovementUpdate();
        AnimationUpdate();
        JumpUpdate();

        inAir = !OnGround;

        if (inAir != inAirOld && !inAir)
        {
            if (trailActive)
            {
                SetTrailActive(false);
            }

            OnLanding();
        }

        inAirOld = inAir;
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
        xVel = Mathf.SmoothDamp(xVel, input.x * speed, ref xVelCurrent, snappiness + airSnappiness + addForceSnappiness);

        addForceSnappiness = Mathf.Clamp01(addForceSnappiness - Time.deltaTime);

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
                    xVel = 0;            
                    break;
                }
            }
        }

        //Movement is applied
        transform.Translate(Vector3.right * xVel * Time.deltaTime * collisionMultiplier);
    }

    void InputUpdate()
    {
        input = disableInput ? Vector2.zero : new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (autoRunTime > 0)
        {
            input.x = autoRunVector.normalized.x;
            autoRunTime -= Time.deltaTime;
        }

        else if(Input.GetButtonDown("Jump") && OnGround && !disableInput && autoRunTime <= 0)
        {
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

    void AnimationUpdate()
    {
        animator.SetBool("Falling", jumpVelocity < 0);

        if (Mathf.Abs(xVel) > 0.15f)
        {
            int direction = inAir ? 2 : 1;
            animator.SetInteger("Dir", direction);

            facingRight = xVel > 0;
            animator.SetBool("FacingRight", facingRight);
            gunSprite.sortingOrder = facingRight ? sprite.sortingOrder + 1 : sprite.sortingOrder - 1;
            gunSprite.flipY = !facingRight;
            moving = facingRight;
        }
        else
        {
            int direction = inAir ? 2 : 0;
            moving = false;

            animator.SetInteger("Dir", direction);

            if (!disableInput)
            {
                animator.SetBool("FacingRight", lookPos.x > 0);
       
                if (lookPos.x > 0)
                {
                    gunSprite.flipY = true;
                    gunSprite.sortingOrder = sprite.sortingOrder + 1;
                }
                else
                {
                    gunSprite.flipY = false;
                    gunSprite.sortingOrder = sprite.sortingOrder - 1;
                }
            }

        }
    }

    public void AutoRun(float time, Vector2 dir)
    {
        autoRunTime = time;
        autoRunVector = dir;
    }

    private void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            jumpVelocity = jumpForce;
            jumptime = 0;

            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Jump", gameObject);

            if (facingRight) {
                animator.Play("a_PlayerJumpRight");
            }
            else
            {
                animator.Play("a_PlayerJumpLeft");
            }
        }
    }

    void OnLanding()
    {
        addForceSnappiness = 0;
    }

    public void AddForce(Vector2 force, bool overwrite)
    {
        addForceSnappiness = 0.5f;

        if (overwrite)
        {
            inAir = true;
            xVel += force.x;
            jumpVelocity = force.y;
        }
        else
        {
            xVel += force.x;
            jumpVelocity += force.y;
        }
    }

    bool OnGround
    {
        get
        {
            Vector2 origin = feetOrigin;

            if (Physics2D.Linecast((Vector2)transform.position + origin, (Vector2)transform.position + origin + Vector2.down * feetLength, obstacleLayermask))
            {
                return true;
            }

            else
            {
                origin.x = -origin.x;
                if (Physics2D.Linecast((Vector2)transform.position + origin, (Vector2)transform.position + origin + Vector2.down * feetLength, obstacleLayermask))
                {
                    return true;
                }
            }

            return false;
        }

    }

    public Vector2 Velocity
    {
        get
        {
            return new Vector2(xVel, jumpVelocity);
        }
    }

    public void SetTrailActive(bool active)
    {
        trailParticles.SetEmission(active);
        trailActive = active;
    }

    public bool InDialogue
    {
        get
        {
            return inDialogue;
        }
        set
        {
            inDialogue = value;
        }
    }


}
