using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiParticleHandler : MonoBehaviour {

    List<ParticleSystem> particles = new List<ParticleSystem>();

	void Start () {
		if (GetComponent<ParticleSystem>())
        {
            particles.Add(GetComponent<ParticleSystem>());
        }

        foreach (var p in GetComponentsInChildren<ParticleSystem>())
        {
            particles.Add(p);
        }
	}
	
    public void SetEmission(bool active)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            var emission = particles[i].emission;
            emission.enabled = active;
        }
    }

}
