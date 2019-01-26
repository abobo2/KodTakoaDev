using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float JumpHeight = 3;

    Vector3 velocity = Vector3.zero;
    float groundPos;
    float sleepThreshold = 0.0125f;
    float bounceCooef = 0.9f;
    float gravity = -9.8f;


    private void Awake()
    {
        groundPos = transform.localPosition.y;
    }

    void OnEnable()
    {
        velocity = transform.forward * Random.Range(0.5f, 1.5f);
        velocity.y = JumpHeight;
    }


    void Update()
    {
        if (velocity.sqrMagnitude > sleepThreshold)
        {
            if (transform.localPosition.y > groundPos)
            {
                velocity.y += gravity * Time.deltaTime;
            }

            transform.position += velocity * Time.deltaTime;

            if (transform.localPosition.y <= groundPos)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, groundPos);
                velocity.y = -velocity.y;
                velocity *= bounceCooef;
            }
        }
    }
}