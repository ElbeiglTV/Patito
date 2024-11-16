using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotHandler : NetworkBehaviour
{
    [SerializeField] NetworkPrefabRef _bulletPrefab;
    [SerializeField] Transform _bulletSpawnTransform;

    public event Action OnShot = delegate { };
    public void Fire()
    {
        if (!HasStateAuthority) return;

        SpawnBullet();

        //RaycastBullet();

        OnShot();
    }

    void SpawnBullet()
    {
        Runner.Spawn(_bulletPrefab, _bulletSpawnTransform.position, _bulletSpawnTransform.rotation);
    }

    void RaycastBullet()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.green, 2f);
        
        Runner.LagCompensation.Raycast(origin: transform.position,
                                        direction: transform.forward,
                                        length: 100,
                                        player: Object.InputAuthority,
                                        hit: out var hitInfo);

        if (hitInfo.Hitbox == null) return;

        if (!hitInfo.Hitbox.transform.root.TryGetComponent(out LifeHandler player)) return;

        player.TakeDamage(25);
    }
}
