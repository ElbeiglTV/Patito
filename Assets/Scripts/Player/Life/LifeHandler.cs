using Fusion;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LifeHandler : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(CurrentLifeChanged))]
    byte CurrentLife { get; set; }

    const byte MAX_LIFE = 150;

    [Networked, OnChangedRender(nameof(DeadStateChanged))]
    NetworkBool IsDead { get; set; }

    //LifeBarItem _lifeBarItem;

    public event Action<bool> OnDeadStateChanged = delegate { };
    public event Action OnResurrect = delegate { };
    public event Action OnDespawn = delegate { };

    public override void Spawned()
    {
        //_lifeBarItem = LifeBarHandler.Instance.CreateNewLifeBarItem(this);

        if (HasStateAuthority)
        {
            CurrentLife = MAX_LIFE;
        }
        else
        {
            CurrentLifeChanged();

            DeadStateChanged();
        }
        RPC_lifeActualizer();
    }

    public void TakeDamage(byte dmg)
    {
        if (IsDead) return;//si está muerto corta aqui

        if (CurrentLife < dmg)//vida: 90-65-40-25-0
        {
            dmg = CurrentLife;
        }
        CurrentLife -= dmg;
        RPC_DamageEfect();
        RPC_lifeActualizer();
        if (CurrentLife == 0)
        {

            if (HasStateAuthority)
            {
                if (HasInputAuthority)
                {
                    NetworkGameManager.Instance.Player1Lose = true;
                }
                else
                {
                    NetworkGameManager.Instance.Player2Lose = true;
                }
            }
           // DisconnectPlayer();

            IsDead = true;
        }
        
    }

    

    [Rpc(RpcSources.StateAuthority,RpcTargets.InputAuthority)]
    public void RPC_DamageEfect()
    {
        Camera.main.GetComponent<MyCamera>().cameraShake.TriggerShake();

       

    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_lifeActualizer()
    {
        GameManager.Instance.LifeFill.fillAmount = CurrentLife / (float)MAX_LIFE;

    }

    void DeadStateChanged()
    {
        GetComponentInParent<HitboxRoot>().HitboxRootActive = !IsDead;
        OnDeadStateChanged(IsDead);
    }

    void CurrentLifeChanged()
    {
        //_lifeBarItem.UpdateFillAmount(CurrentLife / (float)MAX_LIFE);
    }

    void DisconnectPlayer()//desconecta al Proxy
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

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_ShowMessage(string message)
    {
        // Lógica para mostrar el mensaje en la UI del jugador.
        Debug.Log(message);

        // Si usas una UI de Unity, puedes actualizar un Text o Panel aquí.
        // Ejemplo: GameManager.Instance.ShowMessageOnScreen(message);
    }

    

}