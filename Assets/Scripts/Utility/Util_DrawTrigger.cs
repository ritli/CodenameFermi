using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util_DrawTrigger : MonoBehaviour {

    [SerializeField]
    Color color = Color.green;
    new BoxCollider2D collider;

    private void OnDrawGizmos()
    {
        if (collider)
        {
            Gizmos.color = new Color(color.r, color.g, color.b, 0.5f);
            Gizmos.DrawCube(transform.position + (Vector3)collider.offset, collider.size * transform.localScale);
        }
        else
        {
            collider = GetComponent<BoxCollider2D>();
        }
    }
}
