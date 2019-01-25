using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Quaternion targetRotation;

    public Vector2 playerInput;

    public float MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Rotate();
        
    }

    void GetInput()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Rotate()
    { 
        Debug.Log("TODO");
        transform.LookAt(transform.position + Constants.InputToMotionFunction(playerInput) );
//        transform.rotate;
//        Mathf.Atan2()
      
    }

    void Move()
    {
        
        transform.Translate(Constants.InputToMotionFunction(playerInput) * MoveSpeed, Space.World);
    }
}
