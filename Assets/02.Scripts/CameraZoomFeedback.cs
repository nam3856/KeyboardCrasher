using UnityEngine;
using System.Collections;

public class CameraZoomFeedback : MonoBehaviour
{
    private Camera cam;
    private float defaultSize = 5f;
    private Coroutine zoomCoroutine;

    private void Start()
    {
        cam = Camera.main;
        defaultSize = cam.fieldOfView;
    }

    public void DoZoomEffect(float zoomSize = 40f, float duration = 0.1f)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomEffect(zoomSize, duration));
    }

    private IEnumerator ZoomEffect(float zoomSize, float duration)
    {
        cam.fieldOfView = zoomSize;
        yield return new WaitForSeconds(duration);
        cam.fieldOfView = defaultSize;
    }
}
