using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGun : MonoBehaviour {
    private Player player;
    private Rigidbody2D playerBody;
    private Camera cam;

    public float magJumpForce = 1f;
    public float magJumpRange = 2f;
    public float magJumpCooldown = 1;
    float magJumpCooldownElapsed = 0;

    public float magRange = 5f;
    public float magAttachTime = 0.2f;
    public float magPullForce = 1f;
    float magAttachTimeElapsed = 0;
    public LayerMask obstacleAndMagLayermask;

    public Rigidbody2D attachedBody;

    void Start() {
        player = GetComponentInParent<Player>();
        playerBody = GetComponentInParent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update() {
        Vector2 lookPos = cam.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;

        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg + 90);
        transform.localPosition = lookPos.normalized * 0.75f;

        if (Input.GetButton("Fire1"))
        {
            MagHitUpdate();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            MagJump();
        }

        MagPullUpdate();
        magJumpCooldownElapsed = Mathf.Clamp(magJumpCooldownElapsed + Time.deltaTime, 0, magJumpCooldown);
    }

    void MagHitUpdate()
    {
        RaycastHit2D MagHit;

        if (MagHit = Physics2D.Linecast(transform.position, transform.position + transform.up * -magRange, obstacleAndMagLayermask))
        {
            if (MagHit.collider.CompareTag("Magnetic"))
            {
                if (!attachedBody)
                {
                    attachedBody = MagHit.collider.GetComponent<Rigidbody2D>();
                }
                magAttachTimeElapsed = 0;
            }
        }

    }

    void MagPullUpdate()
    {
        magAttachTimeElapsed += Time.deltaTime;

        if (attachedBody && magAttachTimeElapsed > magAttachTime)
        {
            attachedBody.gravityScale = 1f;
            attachedBody = null;
        }

        if (attachedBody)
        {
            attachedBody.gravityScale = 0.4f;
            attachedBody.AddForce((transform.position - attachedBody.transform.position).normalized * magPullForce, ForceMode2D.Impulse);
        }
    }

    void MagJump()
    {
        if (magJumpCooldownElapsed >= magJumpCooldown)
        {
            magJumpCooldownElapsed = 0;
            player.AddForce(transform.up * magJumpForce, true);
        }

    }
}
