using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour {

    public Sprite[] sprites;
    bool spriteRandomized = false;

    Vector3 lastpos = Vector3.zero;

    /*
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (lastpos != transform.position)
        {
            lastpos = transform.position;

            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        }

        lastpos = transform.position;
    }
#endif
    */
    void Start () {
           GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        Destroy(this);
    }

}
