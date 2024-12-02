using Fusion;
using UnityEngine;

[RequireComponent(typeof(ShootHandler))]

public class NewPlayerController : NetworkBehaviour
{
    
    CharacterController _characterController;
    MouseRotate _rotateSystem;
    ShootHandler _shootHandler;
    NetworkInputData inputs;
    Vector3 MoveVector;
    public MeshRenderer meshRenderer;

    public float Speed;
    public float Gravity = 9.81f;


    public override void Spawned()
    {
        _characterController = GetComponent<CharacterController>();
        _shootHandler = GetComponent<ShootHandler>();
        _rotateSystem = new MouseRotate();
        
        

        if (HasStateAuthority)
        {
            if (HasStateAuthority && HasInputAuthority)
            {
                transform.position = NetworkGameManager.Instance.spawnpoint1.position;
            }
            else
            {
                transform.position = NetworkGameManager.Instance.spawnpoint2.position;
            }
        }

    }

    private void Update()
    {
     
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void RPC_SetColor(Color color)
    {
        meshRenderer.material.color = color;
    }


    public override void FixedUpdateNetwork()
    {
        //if (!HasInputAuthority) return;
        if (!GetInput(out inputs)) return; // este motodo agarra el input de el cliente y lo pasa al host el cual ejecuta 
        //las acciones en base a los inputs locales de cada cliente y el estado actual del mundo
        if(NetworkGameManager.Instance.GameStarted)   RPC_SetColor(inputs.color);
        

        if (!inputs.active) return;
        SetMoveVector();
        SetGravity();

        _characterController.Move(MoveVector * Runner.DeltaTime);
        _characterController.transform.rotation = inputs.rotation;


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

        /* if (!isUnderWater)*/
        MoveVector = MoveVector * Speed;
        // else MoveVector = MoveVector * Speed * moveBoost;
    }
}
