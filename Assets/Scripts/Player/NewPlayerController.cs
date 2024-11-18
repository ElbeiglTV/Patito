using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent (typeof(ShootHandler))]

public class NewPlayerController : NetworkBehaviour
{
    CharacterMovement _movement;
    ShootHandler _shootHandler;

    public override void Spawned()
    {
        _movement = GetComponent<CharacterMovement>();
        _shootHandler = GetComponent<ShootHandler>();
    }


    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputs)) return;

        //Movimiento
        //Vector3 dir = Vector3.forward * inputs.axisX * inputs.axisZ;
        Vector3 dir = new Vector3(inputs.axisX,0, inputs.axisZ);
        _movement.Move(dir);
        //Salto
        if (inputs.isJumpPressed)
        {
            _movement.Jump();

        }


        //Disparo
        if (inputs.isFirePressed)
        {
            _shootHandler.Fire();
        }

    }
}
