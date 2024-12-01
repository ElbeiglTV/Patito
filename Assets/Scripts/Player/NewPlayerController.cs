using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Windows;

[RequireComponent (typeof(ShootHandler))]

public class NewPlayerController : NetworkBehaviour
{
    CharacterController _characterController;
    MouseRotate _rotateSystem;
    ShootHandler _shootHandler;
    NetworkInputData inputs;
    Vector3 MoveVector;


    public float Speed;
    public float Gravity = 9.81f;


    public override void Spawned()
    {
        _characterController = GetComponent<CharacterController>();
        _shootHandler = GetComponent<ShootHandler>();
        _rotateSystem = new MouseRotate();
    }

    private void Update()
    {
       
    }

    public override void FixedUpdateNetwork()
    {
   
      
        //if (!HasInputAuthority) return;
        if (!GetInput(out inputs)) return;

        SetMoveVector();
        SetGravity();
         
        _characterController.Move(MoveVector * Runner.DeltaTime);


        //Disparo
        if (inputs.isFirePressed)
        {
            _shootHandler.Fire();
        }

    }
   
    private void SetGravity()
    {
        if (_characterController.isGrounded)
        {
            MoveVector.y = -Gravity * Runner.DeltaTime;
        }
        else
        {
            MoveVector.y -= Gravity * Runner.DeltaTime;
        }
    }
    private void SetMoveVector()
    {

        MoveVector = new Vector3(inputs.axisX, MoveVector.y / Speed, inputs.axisZ);

       /* if (!isUnderWater)*/ MoveVector = MoveVector * Speed;
       // else MoveVector = MoveVector * Speed * moveBoost;
    }
}
