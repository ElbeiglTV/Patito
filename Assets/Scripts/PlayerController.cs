using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class PlayerController : MonoBehaviour
{
    private Vector3 input;
    private Vector3 MoveVector;
    public float Speed;
    public float Gravity= 9.81f;
    CharacterController characterController;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdater();
        SetMoveVector();
        SetGravity();
        characterController.Move(MoveVector*Time.deltaTime);
    }
    private void SetGravity()
    {
        if (characterController.isGrounded)
        {
            MoveVector.y = -Gravity*Time.deltaTime;
        }
        else
        {
            MoveVector.y -= Gravity * Time.deltaTime;
        }
    }
    private void SetMoveVector()
    {

        MoveVector = new Vector3(input.x, MoveVector.y/Speed, input.z);
        MoveVector = MoveVector * Speed;
    }

    private void InputUpdater() 
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        

    }
    


}
