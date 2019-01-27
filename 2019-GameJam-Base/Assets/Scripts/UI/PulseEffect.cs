using UnityEngine;
using System.Collections;

public class PulseEffect : MonoBehaviour
{
    float t;
    public float length = 1.2f;
    public float speed = 0.3f;

    public float startTimeOut = 0;

    Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {

        if (startTimeOut > 0)
        {
            startTimeOut -= Time.deltaTime;
            return;
        }

        t += Time.deltaTime * speed;

        float pong = Mathf.PingPong(t, length - 1);

        transform.localScale = new Vector3(pong + originalScale.x, pong + originalScale.y, originalScale.z);
    }

    void OnDisable()
    {
        transform.localScale = originalScale;
    }

    void OnDestroy()
    {
        transform.localScale = originalScale;
    }

}