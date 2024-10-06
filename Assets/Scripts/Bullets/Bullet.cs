using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    
    [SerializeField] float _speed = 10f;
    [SerializeField] float _damage = 10f;
    [SerializeField] float _lifeTime = 5f;

    TickTimer _lifetickTimer;

    public override void Spawned()
    {
        _lifetickTimer = TickTimer.CreateFromSeconds(Runner,_lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * _speed * Runner.DeltaTime;

        if (!_lifetickTimer.Expired(Runner)) return;

        Runner.Despawn(Object);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != 3) return;
        PlayerController player;

        if (collision.gameObject.TryGetComponent(out player))
        {
           // player.TakeDamage(_damage);
           Debug.Log("Player Hit");
        }
    }


}
