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

    // Use this for initialization
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray r = new Ray(target.position, -transform.forward);
        Vector3 finalPos = r.GetPoint(distance) + transform.TransformVector(Screenshake.ScreenshakeVector);
        transform.position = Vector3.MoveTowards(finalPos, finalPos, .1f);
//        Vector3 targetCamPos = target.position + offset;
//        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
