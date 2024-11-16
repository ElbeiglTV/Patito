using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(ShotHandler))]
[RequireComponent(typeof(LifeHandler))]
public class PlayerController : NetworkBehaviour
{
    NetworkCharacterControllerCustom _movementHandler;
    ShotHandler _shotHandler;

    public override void Spawned()
    {
        _movementHandler = GetComponent<NetworkCharacterControllerCustom>();
        _shotHandler = GetComponent<ShotHandler>();

        var lifeHandler = GetComponent<LifeHandler>();

        lifeHandler.OnDeadStateChanged += (b) =>
        {
            enabled = !b;
        };

        lifeHandler.OnResurrect += () =>
        {
            _movementHandler.Teleport(transform.position + Vector3.up * 2.5f);
        };
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputs)) return;

        //Movimiento
        var direction = Vector3.forward * inputs.movementInput;
        _movementHandler.Move(direction);

        //Disparo
        if (inputs.isFirePressed)
        {
            _shotHandler.Fire();
        }

        //Salto
        if (inputs.networkButtons.IsSet(MyButtons.Jump))
        {
            _movementHandler.Jump();
        }
    }
}
