using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public Transform lookAtTransform;
    Camera mainCamera;
    public string sceneName;
    public Vector2 direction;
    public bool waitForDialogue;

    public float transitionTime = 5f;

    private void OnValidate()
    {
        if (!lookAtTransform)
        {
            lookAtTransform = new GameObject("Look Position").transform;
            lookAtTransform.transform.parent = transform;
            lookAtTransform.position = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (lookAtTransform)
        {
            Gizmos.DrawWireCube(lookAtTransform.position, Vector3.one);
        }

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction.normalized);
        Gizmos.DrawLine(transform.position + Vector3.up + (Vector3)direction.normalized * 0.5f, transform.position + (Vector3)direction.normalized);
        Gizmos.DrawLine(transform.position + Vector3.down + (Vector3)direction.normalized * 0.5f, transform.position + (Vector3)direction.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeScene(sceneName, 5f));
        }
    }

    IEnumerator ChangeScene(string sceneName, float transitionTime)
    {
        yield return new WaitForEndOfFrame();

        Manager.GetCamera.TimedLook(5f, lookAtTransform.position, Camera.main.orthographicSize + 3);
        Manager.GetPlayer.AutoRun(5f, direction);

        yield return new WaitForSeconds(transitionTime-1);

        Manager.GetFade.FadeOut(1, Color.white);
        yield return new WaitForSeconds(1);

        if (!waitForDialogue)
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
