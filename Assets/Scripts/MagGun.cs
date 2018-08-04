using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGun : MonoBehaviour {
    private Player player;
    private Rigidbody2D playerBody;
    private Camera cam;
    private MultiParticleHandler attractorParticles;

    [Header("References")]
    public GameObject attractParticlesPrefab;
    public GameObject repulsorParticlesPrefab;
    public GameObject attachParticlesPrefab;
    public GameObject magGlow;
    public SpriteRenderer gunSprite;

    Animator gunAnimator;

    [Header("Audio")]
    public AudioClip magLoop;
    public AudioClip magStart;
    public AudioClip magStop;
    public AudioClip magFire;
    public AudioClip magFireJump;
    public float volume;

    new AudioSource audio;

    [Header("Vars")]

    public float magJumpForce = 1f;
    public float magJumpRange = 2f;
    public float magJumpCooldown = 1;
    float magJumpCooldownElapsed = 0;

    bool canMagPull = true, magPulling = false;
    private bool propFlying;
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

        audio = GetComponent<AudioSource>();

        attractorParticles = Instantiate(attractParticlesPrefab, transform.position, attractParticlesPrefab.transform.rotation).GetComponent<MultiParticleHandler>();

    }

    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        lookPos = mousePos - transform.parent.position;

        if (!player.disableInput)
        {
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
        }

        MagPullUpdate();
        magJumpCooldownElapsed = Mathf.Clamp(magJumpCooldownElapsed + Time.deltaTime, 0, magJumpCooldown);
    }

    void CancelMagPull(bool resetCanPull)
    {
        magPulling = !resetCanPull;
        canMagPull = resetCanPull;
    }

    void MagHitUpdate()
    {
        magPulling = true;
        propFlying = false;

        Collider2D[] MagHits;

        MagHits = Physics2D.OverlapCircleAll(mousePos, magRange, obstacleAndMagLayermask);

        Vector3 distance = mousePos - transform.position;

        //Check for propflying
        if (Mathf.Abs(distance.x) < 0.5f && distance.y > -1.5f && distance.y < -0.1f)
        {
            propFlying = true;

        }
        else
        {
            propFlying = false;

            for (int i = 0; i < MagHits.Length; i++)
            {
                if (MagHits[i].CompareTag("Magnetic"))
                {
                    MagHits[i].GetComponent<Rigidbody2D>().AddForce((mousePos - MagHits[i].transform.position).normalized * magPullForce, ForceMode2D.Impulse);

                }
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

    bool magStateSwapped;

    void MagPullUpdate()
    {
        if (magPulling && !propFlying)
        {
            attractorParticles.transform.localScale = Vector3.Lerp(attractorParticles.transform.localScale, Vector3.one, Time.deltaTime * 5f);

            attractorParticles.SetEmission(true);

            if (!magStateSwapped)
            {
                audio.PlayOneShot(magStart, volume);
                audio.clip = magLoop;
                audio.Play((ulong)0.5);
                magStateSwapped = true;
            }
        }

        else
        {
            attractorParticles.transform.localScale = Vector3.Lerp(attractorParticles.transform.localScale, Vector3.zero, Time.deltaTime * 15f);
            attractorParticles.SetEmission(false);

            if (magStateSwapped)
            {
                audio.Stop();
                audio.PlayOneShot(magStop, volume);

                magStateSwapped = false;
            }
        }

        magAttachTimeElapsed += Time.deltaTime;
    }

    void MagRepulse()
    {
        if (magJumpCooldownElapsed >= magJumpCooldown)
        {
            Manager.GetCamera.SetScreenShake(0.1f, 0.2f);

            if (player.inAir)
            {
                audio.PlayOneShot(magFireJump);
            }
            else
            {
                audio.PlayOneShot(magFire);
            }

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
