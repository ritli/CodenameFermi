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
            Gizmos.color = color;
            Gizmos.DrawWireCube(transform.position + (Vector3)collider.offset, collider.size * transform.localScale);
        }
        else
        {
            collider = GetComponent<BoxCollider2D>();
        }
    }
}
