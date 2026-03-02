using Fusion;
using UnityEngine;
using System;

public class LifeHandler : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnLifeChanged))]
    public byte CurrentLife { get; set; }

    [Networked, OnChangedRender(nameof(OnDeadChanged))]
    public NetworkBool IsDead { get; set; }

    const byte MAX_LIFE = 150;

    public event Action<bool> OnDeadStateChanged = delegate { };
    public event Action OnDespawn = delegate { };

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CurrentLife = MAX_LIFE;
            IsDead = false;
        }

        // Forzar actualización visual inicial
        OnLifeChanged();
        OnDeadChanged();
    }

    public void TakeDamage(byte dmg)
    {
        // SOLO EL HOST MODIFICA ESTADO
        if (!HasStateAuthority) return;
        if (IsDead) return;

        if (CurrentLife <= dmg)
            CurrentLife = 0;
        else
            CurrentLife -= dmg;

        if (CurrentLife == 0)
        {
            IsDead = true;

            // Notificamos al GameManager correctamente
            NetworkGameManager.Instance.RegisterPlayerDeath(Object.InputAuthority);
        }
    }

    void OnLifeChanged()
    {
        // Solo actualiza la UI del dueño del jugador
        if (!Object.HasInputAuthority) return;

        GameManager.Instance.LifeFill.fillAmount =
            CurrentLife / (float)MAX_LIFE;
    }

    void OnDeadChanged()
    {
        GetComponentInParent<HitboxRoot>().HitboxRootActive = !IsDead;
        OnDeadStateChanged(IsDead);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawn();
    }
}