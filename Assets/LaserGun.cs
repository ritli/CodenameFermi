using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour {
    private Camera cam;
    LineRenderer line;

    public float laserRange = 10;

	void Start () {
        cam = Camera.main;
        line = GetComponentInChildren<LineRenderer>();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 lookPos = cam.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;

            RaycastHit2D hit = Physics2D.Linecast((Vector2)transform.position + lookPos.normalized, (Vector2)transform.position + lookPos.normalized * laserRange);

            line.SetPosition(0, transform.position + (Vector3)lookPos.normalized);

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
                line.SetPosition(1, lookPos * laserRange);
            }

            line.startColor = Color.red;
            line.endColor = Color.red;
        }

        line.startColor = Color.Lerp(line.startColor, Color.clear, Time.deltaTime * 2f);
        line.endColor = Color.Lerp(line.endColor, Color.clear, Time.deltaTime * 2f);
    }
}
