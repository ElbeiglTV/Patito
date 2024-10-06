using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class PlayerController : NetworkBehaviour
{
    public Transform gunAnchor;
    public GameObject GunEquiped;


    private Vector3 input;
    private Vector3 MoveVector;

    public float Speed;
    public float Gravity= 9.81f;

    MouseRotate RotateSystem;
    CharacterController characterController;


    // Start is called before the first frame update
    void Start()
    {
        InitializePlayer();
    }
    
    private void Update()
    {
        if (!HasStateAuthority) return;
        InputUpdater();
        
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        SetMoveVector();
        SetGravity();
        RotateSystem.RotatePlayer(transform);
        characterController.Move(MoveVector*Runner.DeltaTime);
    }
    private void SetGravity()
    {
        if (characterController.isGrounded)
        {
            MoveVector.y = -Gravity*Runner.DeltaTime;
        }
        else
        {
            MoveVector.y -= Gravity * Runner.DeltaTime;
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
    public void InitializePlayer()
    {
        RotateSystem = new MouseRotate();
        characterController = GetComponent<CharacterController>();
    }



}
