using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarmapHandler : MonoBehaviour {

    public GameObject starmapPoint;

    public int count, stages;

    [FMODUnity.EventRef]
    public string bleepEvent;

	void Start () {

        Transform parent = transform.Find("Points");

        for (int i = 0; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }

        StartCoroutine(ShowPointsRoutine());
    }
	


    IEnumerator ShowPointsRoutine()
    {
        yield return new WaitForSecondsRealtime(2f);

        Transform parent = transform.Find("Points");

        float randBase = 0.3f;

        for (int i = 0; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(true);
            parent.GetChild(i).GetComponent<Animator>().enabled = true;

            FMODUnity.RuntimeManager.PlayOneShot(bleepEvent);

            yield return new WaitForSecondsRealtime(Mathf.Clamp(Random.Range(randBase, randBase* 1.5f), 0.06f,randBase * 2));
            randBase -= Time.unscaledDeltaTime * 0.5f;
        }
    }
}


