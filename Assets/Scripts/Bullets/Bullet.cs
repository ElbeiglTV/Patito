using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    
    [SerializeField] float _speed = 10f;

    public override void Spawned()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * _speed * Runner.DeltaTime;
    }


}
