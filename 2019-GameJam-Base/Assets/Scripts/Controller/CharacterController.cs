using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Quaternion targetRotation;

    public Vector2 playerInput;

    public float MoveSpeed;

    public float RotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Rotate();
        Move();
        
    }

    void GetInput()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Rotate()
    { 
        Debug.Log("TODO");
        if (playerInput.magnitude > 0.2f)
        {
            Quaternion targetRot = Quaternion.LookRotation(Constants.InputToMotionFunction(playerInput));
            transform.rotation = (Quaternion.RotateTowards(transform.rotation, targetRot, RotationSpeed) );
        }
//        transform.rotate;
//        Mathf.Atan2()
      
    }

    void Move()
    {
        
        transform.Translate(transform.forward * MoveSpeed * playerInput.magnitude, Space.World);
    }
}
