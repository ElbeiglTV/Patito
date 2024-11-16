using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class Bullet : NetworkBehaviour
{
    
    [SerializeField] float _speed = 10f;
    public float damage = 10f;
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
    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void RPC_CuackShootBullet(float multiply)
    {
        CuackShootBullet(multiply);
    }
    public void CuackShootBullet(float multiply)
    {
        damage *= multiply;
        transform.localScale *= multiply;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!HasStateAuthority) return;
         
        if (collision.collider.gameObject.layer == 3)
        {
            collision.collider.gameObject.GetComponent<PlayerControllerOld>().RPC_TakeDamage(damage);
           // player.TakeDamage(_damage);
           Debug.Log("Player Hit");
           Runner.Despawn(Object);
        }

    }


}
