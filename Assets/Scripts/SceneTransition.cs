using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour, ITrigger {

    public Transform lookAtTransform;
    public bool useLookAt = true;

    Camera mainCamera;
    public string sceneName;
    public Vector2 direction;
    public bool waitForDialogue;

    public float transitionTime = 5f;

    FMOD.Studio.EventInstance muteInstance;

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
            StartCoroutine(ChangeScene(sceneName, transitionTime));
        }
    }

    public static string GetSceneName
    {
        get
        {
            return SceneManager.GetActiveScene().ToString();
        }
    }

    IEnumerator ChangeScene(string sceneName, float transitionTime)
    {
        yield return new WaitForEndOfFrame();

        if (useLookAt)
        {
            Manager.GetCamera.TimedLook(transitionTime, lookAtTransform.position, Camera.main.orthographicSize + 3);
        }

        if (direction.x != 0)
        {
            Manager.GetPlayer.AutoRun(transitionTime, direction);
        }

        yield return new WaitForSeconds(transitionTime * 0.5f);

        Manager.GetFade.FadeOut(transitionTime * 0.5f, Color.white);
        muteInstance = FMODUnity.RuntimeManager.CreateInstance("snapshot:/MuteMusic");
        muteInstance.start();

        yield return new WaitForSeconds(transitionTime * 0.51f);

        if (!waitForDialogue)
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        muteInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        StartCoroutine(DelayedChangeScene(0.2f));
    }

    public IEnumerator DelayedChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void StartTrigger()
    {
        StartCoroutine(ChangeScene(sceneName, transitionTime));
    }

    public void OnEventFinished()
    {
    }
}
