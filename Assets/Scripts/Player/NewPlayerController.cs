using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(CharacterMovement))]

public class NewPlayerController : NetworkBehaviour
{
    CharacterMovement _movement;

    public override void Spawned()
    {
        _movement = GetComponent<CharacterMovement>();
    }


    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputs)) return;

        //Movimiento
        Vector3 dir = Vector3.forward * inputs.axisX*inputs.axisZ;
        _movement.Move(dir);
        //Salto
        if (inputs.isJumpPressed)
        {
            _movement.Jump();

        }


        //Disparo


    }
}
