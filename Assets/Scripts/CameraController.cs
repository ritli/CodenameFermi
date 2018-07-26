using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    float m_initialZoom;
    float m_targetZoom;
    Vector3 m_position;
    Vector3 m_lastPosition;
    float m_shakeAmount = 0;
    float m_shakeTime;
    public float m_zoomSpeed = 6;
    public float m_interpolateVal = 0.5f;
    [Range(0, 1)]
    public float m_mouseExtendMultiplier = 1;
    bool m_TimedLookActive = false;
    Vector3 m_timedLookPos;

    Transform target;
    new Camera camera;
    public bool m_interpolateCamera = true;

    public float m_zZoomLevel;

    private void Start()
    {
        target = Manager.GetPlayer.transform;

        camera = GetComponent<Camera>();

        m_initialZoom = camera.orthographicSize;
        m_targetZoom = m_initialZoom;


        //Cursor.visible = false;
    }


    void Update()
    {
        if (!m_TimedLookActive)
        {
            GetPlayerPosition(ref m_position);
            GetAddedMousePos(ref m_position);
        }

        else
        {
            GetLookPosition(ref m_position);
        }

        UpdateZoom();
        MoveToPosition(m_position);

    }

    void UpdateZoom()
    {
        camera.orthographicSize = Mathf.SmoothStep(camera.orthographicSize, m_targetZoom, Time.unscaledDeltaTime * m_zoomSpeed);
    }

    void GetAddedMousePos(ref Vector3 position)
    {
        float extendPercent = (new Vector3(Screen.width * 0.5f, Screen.height * 0.5f) - Input.mousePosition).magnitude / (new Vector3(Screen.width * 0.5f, Screen.height * 0.5f) - new Vector3(Screen.width, Screen.height)).magnitude;

        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition) - target.position;

        position += mousePos.normalized * extendPercent * 5f * m_mouseExtendMultiplier;
        position.z = m_zZoomLevel;
    }

    void GetPlayerPosition(ref Vector3 position)
    {
        position = target.position + Vector3.up * 2f;
        position.z = m_zZoomLevel;
    }

    void GetLookPosition(ref Vector3 position)
    {
        position = m_timedLookPos;
        position.z = m_zZoomLevel;
    }

    void GetShakePosition(ref Vector3 position)
    {
        position += (Vector3)Random.insideUnitCircle * m_shakeAmount;
    }

    void MoveToPosition(Vector3 position)
    {
        if (m_interpolateCamera)
        {
            Vector3 pos = Vector3.Lerp(m_lastPosition, position, m_interpolateVal);
            GetShakePosition(ref pos);
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 50f);
        }
        else
        {
            transform.position = position;
        }

        m_lastPosition = transform.position;
    }

    void StopScreenShake()
    {
        m_shakeAmount = 0;
    }

    public void SetScreenShake(float time, float amount)
    {
        if (amount != 0)
        {
            Invoke("StopScreenShake", time);
        }

        m_shakeAmount = amount;
    }

    public void TimedLook(float time, Vector2 position, float zoomLevel)
    {
        m_timedLookPos = position;
        StartCoroutine(TimedLookRoutine(time, zoomLevel));
    }

    IEnumerator TimedLookRoutine(float time, float zoomLevel, bool unscaled)
    {
        m_targetZoom = zoomLevel;
        m_TimedLookActive = true;

        if (unscaled)
        {
            yield return new WaitForSecondsRealtime(time);
        }
        else
        {
            yield return new WaitForSeconds(time);
        }

        m_targetZoom = m_initialZoom;
        m_TimedLookActive = false;
    }

    IEnumerator TimedLookRoutine(float time, float zoomLevel)
    {
        m_targetZoom = zoomLevel;
        m_TimedLookActive = true;

        yield return new WaitForSeconds(time);

        m_targetZoom = m_initialZoom;
        m_TimedLookActive = false;
    }

    public void TimedLookToggle(bool enabled, Vector2 position)
    {
        m_timedLookPos = position;
        m_TimedLookActive = enabled;
    }
    /*

    public void FlashVignetteColor(Color color, float time)
    {
        StartCoroutine(FlashVignetteRoutine(color, time));
    }
    IEnumerator FlashVignetteRoutine(Color color, float time)
    {
        var vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.rounded.Override(true);
        vignette.enabled.Override(true);
        vignette.intensity.Override(0.275f);
        vignette.color.Override(color);
        vignette.smoothness.Override(1);
        vignette.roundness.Override(1);

        var volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, vignette);

        float elapsedTime = 0;

        while (elapsedTime < time * 0.1f)
        {
            volume.weight = Mathf.Lerp(0, 1, elapsedTime / time * 0.1f);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        elapsedTime = 0;

        while (elapsedTime < time * 0.9f)
        {
            volume.weight = Mathf.Lerp(1, 0, elapsedTime / time * 0.9f);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        RuntimeUtilities.DestroyVolume(volume, false);
    }
    */
}
