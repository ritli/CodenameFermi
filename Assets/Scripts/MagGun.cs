﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGun : MonoBehaviour {
    private Player player;
    private Rigidbody2D playerBody;
    private Camera cam;
    private ParticleSystem attractorParticles;
    public GameObject attractParticlesPrefab;
    public GameObject repulsorParticlesPrefab;
    public GameObject attachParticlesPrefab;
    public GameObject magGlow;

    public SpriteRenderer gunSprite;
    Animator gunAnimator;

    public float magJumpForce = 1f;
    public float magJumpRange = 2f;
    public float magJumpCooldown = 1;
    float magJumpCooldownElapsed = 0;

    bool canMagPull = true, magPulling = false;

    public float magRange = 5f;
    public float magAttachTime = 0.2f;
    public float magPullForce = 1f;
    float magAttachTimeElapsed = 0;
    public LayerMask obstacleAndMagLayermask;

    public List<Rigidbody2D> attachedBodies = new List<Rigidbody2D>();
    private Vector3 mousePos;
    private Vector3 lookPos;
    public float repulseRange = 5;
    public float repulseForce = 10;

    void Start() {
        gunAnimator = gunSprite.GetComponent<Animator>();
        player = GetComponentInParent<Player>();
        playerBody = GetComponentInParent<Rigidbody2D>();
        cam = Camera.main;

        attractorParticles = Instantiate(attractParticlesPrefab, transform.position, attractParticlesPrefab.transform.rotation).GetComponent<ParticleSystem>();
        attractorParticles.gameObject.SetActive(false);
    }

    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        lookPos = mousePos - transform.parent.position;

        //Gun.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg + 180);

        if (!magPulling && player.moving)
        {
            float facingX = -1;

            if (player.facingRight)
            {
                facingX = -facingX;
            }

            gunSprite.transform.localRotation = Quaternion.Lerp(gunSprite.transform.localRotation, Quaternion.Euler(0, 0, Mathf.Atan2(-0.65f, facingX) * Mathf.Rad2Deg + 180), Time.deltaTime * 8f);
        }
        else
        {
            gunSprite.transform.localRotation = Quaternion.Lerp(gunSprite.transform.localRotation, Quaternion.Euler(0, 0, Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg + 180), Time.deltaTime * 8f);
        }

        if (Input.GetButton("Fire1"))
        {
            if (canMagPull)
            {
                gunAnimator.Play("Charge");
                attractorParticles.transform.position = mousePos;
                attractorParticles.gameObject.SetActive(true);
                MagHitUpdate();
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            gunAnimator.Play("Idle");

            CancelMagPull(true);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (!magPulling)
            {
                MagRepulse();
            }
        }

        MagPullUpdate();
        magJumpCooldownElapsed = Mathf.Clamp(magJumpCooldownElapsed + Time.deltaTime, 0, magJumpCooldown);
    }

    void CancelMagPull(bool resetCanPull)
    {
        magPulling = !resetCanPull;

        attractorParticles.gameObject.SetActive(false);
        canMagPull = resetCanPull;
    }

    void MagHitUpdate()
    {
        magPulling = true;

        Collider2D[] MagHits;

        MagHits = Physics2D.OverlapCircleAll(mousePos, magRange, obstacleAndMagLayermask);

        for (int i = 0; i < MagHits.Length; i++)
        {
            if (MagHits[i].CompareTag("Magnetic"))
            {
                MagHits[i].GetComponent<Rigidbody2D>().AddForce((mousePos - MagHits[i].transform.position).normalized * magPullForce, ForceMode2D.Impulse);

            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            int firstIndex = -1;

            for (int i = 0; i < MagHits.Length; i++)
            {
                if (MagHits[i].CompareTag("Magnetic"))
                {
                    if (firstIndex == -1)
                    {
                        firstIndex = i;
                    }
                    else
                    {
                        MagHits[i].gameObject.AddComponent<FixedJoint2D>().connectedBody = MagHits[firstIndex].GetComponent<Rigidbody2D>();
                    }

                    Instantiate(magGlow, MagHits[i].transform, false);
                }
            }

            Instantiate(
                attachParticlesPrefab,
                gunSprite.transform.position - gunSprite.transform.right * 0.35f,
                Quaternion.Euler(0, 0, gunSprite.transform.rotation.eulerAngles.z - 180), player.transform);


            CancelMagPull(false);
        }

        magAttachTimeElapsed = 0;
    }

    void MagPullUpdate()
    {
        magAttachTimeElapsed += Time.deltaTime;
        /*
        if (attachedBody && magAttachTimeElapsed > magAttachTime)
        {
            attachedBody.gravityScale = 1f;
            attachedBody = null;
        }

        if (attachedBody)
        {
            attachedBody.gravityScale = 0.4f;
            attachedBody.AddForce((mousePos - attachedBody.transform.position).normalized * magPullForce, ForceMode2D.Impulse);
        }
        */
    }

    void MagRepulse()
    {
        if (magJumpCooldownElapsed >= magJumpCooldown)
        {
            gunSprite.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg + 180);

            Collider2D[] colliderHits = Physics2D.OverlapAreaAll(transform.position + gunSprite.transform.up, transform.position - gunSprite.transform.right * repulseRange - gunSprite.transform.up);

            Instantiate(
                repulsorParticlesPrefab, 
                gunSprite.transform.position - gunSprite.transform.right * 0.45f,
                Quaternion.Euler(0,0, gunSprite.transform.rotation.eulerAngles.z - 180), player.transform);

            for (int i = 0; i < colliderHits.Length; i++)
            {
                if (colliderHits[i].CompareTag("Magnetic"))
                {
                    if (colliderHits[i].GetComponent<FixedJoint2D>())
                    {
                        Destroy(colliderHits[i].GetComponent<FixedJoint2D>());
                    }

                    colliderHits[i].GetComponent<Rigidbody2D>().AddForce(-gunSprite.transform.right * repulseForce, ForceMode2D.Impulse);

                    if (colliderHits[i].transform.childCount > 0)
                    {
                        Destroy(colliderHits[i].transform.GetChild(0).gameObject);
                    }
                }
            } 

            magJumpCooldownElapsed = 0;

            if (player.inAir)
            {
                player.AddForce(-lookPos.normalized * magJumpForce, true);
            }
        }

    }
}
