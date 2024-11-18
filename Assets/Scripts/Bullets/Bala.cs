using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkRigidbody3D))]

public class Bala : NetworkBehaviour
{
    [SerializeField] byte _dmg = 25;
    [SerializeField] byte _balaSpeed = 50;
    TickTimer _lifeTimer = TickTimer.None;//timer para vida

    public override void Spawned()
    {
        GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(transform.forward * _balaSpeed, ForceMode.VelocityChange);//impulso hacia adelante

        if (HasStateAuthority)
        {
            _lifeTimer = TickTimer.CreateFromSeconds(Runner, 2);
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (!_lifeTimer.Expired(Runner)) return;

        Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !HasStateAuthority) return;

        /*if (other.TryGetComponent(out LifeHandler player))
        {
            player.TakeDamage(_dmg);
        }*/
        Runner.Despawn(Object);
    }
}
