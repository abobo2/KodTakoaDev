using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    public float distance;
    Vector3 offset;

    private float minZoom = 10;
    private float maxZoom = 20;

    private float zoomMultiplier = 20;
    private Camera mainCamera;
    private Coroutine zoomCoroutine;
    
    private void Start()
    {
        mainCamera = Camera.main;
        offset = transform.position - target.position;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (mainCamera.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * zoomMultiplier) > maxZoom ||
                mainCamera.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * zoomMultiplier) < minZoom)
            {
                return;
            }

            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }

            zoomCoroutine = StartCoroutine(ZoomCamera(mainCamera.orthographicSize, mainCamera.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * zoomMultiplier)));
        }
    }

    private IEnumerator ZoomCamera(float startZoom, float destinationZoom, float time = 0.2f)
    {
        float currentTime = 0f;

        while (currentTime < time)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, destinationZoom, currentTime / time);
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        zoomCoroutine = null;
    }

    private void FixedUpdate()
    {
        Ray r = new Ray(target.position, -transform.forward);
        Vector3 finalPos = r.GetPoint(distance) + transform.TransformVector(Screenshake.ScreenshakeVector);
        transform.position = Vector3.MoveTowards(finalPos, finalPos, .1f);
//        Vector3 targetCamPos = target.position + offset;
//        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
