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

        Rigidbody rb = Object.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * _speed;
    }

    public override void FixedUpdateNetwork()
    {
       
        if (!_lifetickTimer.Expired(Runner)) return;

        Runner.Despawn(Object);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!HasStateAuthority) return;
         
        if (collision.collider.gameObject.layer == 3)
        {
            collision.collider.gameObject.GetComponent<PlayerController>().RPC_TakeDamage(_damage);
           // player.TakeDamage(_damage);
           Debug.Log("Player Hit");
           Runner.Despawn(Object);
        }

    }


}
