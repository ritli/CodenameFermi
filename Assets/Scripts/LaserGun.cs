using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour {
    private Camera cam;
    LineRenderer line;
    public Transform gunSprite;
    public ParticleSystem particles;
    public float laserRange = 10;

    const float timeTillLaserFade = 0.4f;
    float timeSinceLaserFired = 0;

	void Start () {
        cam = Camera.main;
        line = GetComponentInChildren<LineRenderer>();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 lookPos = cam.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;

            RaycastHit2D hit = Physics2D.Linecast((Vector2)gunSprite.position - (Vector2)gunSprite.right, (Vector2)gunSprite.position - (Vector2)gunSprite.right * laserRange);

            timeSinceLaserFired = 0;
            line.SetPosition(0, gunSprite.position - gunSprite.right * 0.25f);
            particles.Clear();
            particles.Play(true);
            if (hit)
            {
                if (hit.collider.GetComponent<Destructible>())
                {
                    hit.collider.GetComponent<Destructible>().Destroy();
                }

                line.SetPosition(1, (Vector3)hit.point);
            }
            else
            {
                line.SetPosition(1, gunSprite.position - gunSprite.right * laserRange);
            }

            line.startColor = Color.red;
            line.endColor = Color.red;
        }

        if (timeSinceLaserFired > timeTillLaserFade)
        {
            particles.Stop(true,ParticleSystemStopBehavior.StopEmitting);
            line.startColor = Color.Lerp(line.startColor, Color.clear, Time.deltaTime * 2f);
            line.endColor = Color.Lerp(line.endColor, Color.clear, Time.deltaTime * 2f);
        }
        else
        {
            timeSinceLaserFired += Time.deltaTime;
        }

   }
}
