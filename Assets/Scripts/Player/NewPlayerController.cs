using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(CharacterMovement))]
public class NewPlayerController : NetworkBehaviour
{
    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputs)) return;

        //Movimiento
        Vector3 dir = Vector3.forward * inputs.axisX*inputs.axisZ;

        //Disparo


        //Salto

    }
}
