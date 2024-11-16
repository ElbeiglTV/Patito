using Fusion;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(CurrentLifeChanged))]
    byte CurrentLife { get; set; }

    const byte MAX_LIFE = 100;

    byte _maxDeaths = 3;

    [Networked, OnChangedRender(nameof(DeadStateChanged))]
    NetworkBool IsDead { get; set; }

    LifeBarItem _lifeBarItem;

    public event Action<bool> OnDeadStateChanged = delegate { };
    public event Action OnResurrect = delegate { };
    public event Action OnDespawn = delegate { };

    public override void Spawned()
    {
        _lifeBarItem = LifeBarHandler.Instance.CreateNewLifeBarItem(this);

        if (HasStateAuthority)
        {
            CurrentLife = MAX_LIFE;
        }
        else
        {
            CurrentLifeChanged();

            DeadStateChanged();
        }

    }

    public void TakeDamage(byte dmg)
    {
        if (IsDead) return;

        if (CurrentLife < dmg)
        {
            dmg = CurrentLife;
        }
        CurrentLife -= dmg;

        if (CurrentLife > 0) return;

        _maxDeaths--;

        if (_maxDeaths == 0)
        {
            DisconnectPlayer();
            return;
        }

        IsDead = true;
        StartCoroutine(Server_ResurrectCooldown());
    }

    IEnumerator Server_ResurrectCooldown()
    {
        yield return new WaitForSeconds(2);

        Server_Resurrect();
    }

    void Server_Resurrect()
    {
        OnResurrect();
        CurrentLife = MAX_LIFE;
        IsDead = false;
    }

    void DeadStateChanged()
    {
        GetComponentInParent<HitboxRoot>().HitboxRootActive = !IsDead;
        OnDeadStateChanged(IsDead);
    }

    void CurrentLifeChanged()
    {
        _lifeBarItem.UpdateFillAmount(CurrentLife / (float)MAX_LIFE);
    }

    void DisconnectPlayer()
    {
        if (!Object.HasInputAuthority)
        {
            Runner.Disconnect(Object.InputAuthority);
        }

        Runner.Despawn(Object);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawn();
    }
}
