using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public ParticleSystem destroyParticles;

    public void Destroy()
    {
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.05f);

        Instantiate(destroyParticles, transform.position - transform.forward, destroyParticles.transform.rotation);
        Destroy(gameObject);
    }
}
