using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : NetworkBehaviour
{
    public bool Active;

    #region Movement
    private Vector3 input;
    private Vector3 MoveVector;

    public float Speed;
    public float Gravity = 9.81f;

    MouseRotate RotateSystem;
    CharacterController characterController;
    #endregion

    #region Life
    public float MaxHealth = 100f;

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public float CurrentHealth { get; set; }
    #endregion

    #region Camera
    public MyCamera myCamera;
    public Transform RenderCameraTransformTarget;
    #endregion

    public PlayerReference playerReference;
    public PlayerEvents playerEvents;

    public override void Spawned()
    {
        InitializePlayer();

        if (HasStateAuthority)
        {
            CurrentHealth = MaxHealth;
        }

        OnHealthChanged();

        if (!HasInputAuthority) return;

        UnityEngine.Camera.main.GetComponent<MyCamera>().target = transform;
        myCamera = UnityEngine.Camera.main.GetComponent<MyCamera>();

        Active = false;
    }

    private void Update()
    {
        if (!HasInputAuthority) return;

        if (NetworkGameManager.Instance.GameEnded)
            return;

        if (!Active)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && NetworkGameManager.Instance.GameStarted)
                Active = true;

            return;
        }

        input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (Input.GetKey(KeyCode.Mouse0))
            Shoot();
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        SetMoveVector();
        SetGravity();
        RotateSystem.RotatePlayer(transform);
        characterController.Move(MoveVector * Runner.DeltaTime);
    }

    private void SetGravity()
    {
        if (characterController.isGrounded)
            MoveVector.y = -Gravity * Runner.DeltaTime;
        else
            MoveVector.y -= Gravity * Runner.DeltaTime;
    }

    private void SetMoveVector()
    {
        MoveVector = new Vector3(input.x, MoveVector.y / Speed, input.z) * Speed;
    }

    void InitializePlayer()
    {
        RotateSystem = new MouseRotate();
        characterController = GetComponent<CharacterController>();
    }

    #region Shooting

    void Shoot()
    {
        if (!HasStateAuthority) return;

        Runner.Spawn(
            playerReference.cuackBulletPrefab,
            playerReference.cuackShootRoot.position,
            playerReference.cuackShootRoot.rotation
        );
    }

    #endregion

    #region Damage

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float damage)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (!HasStateAuthority) return;

        NetworkGameManager.Instance.RegisterPlayerDeath(Object.InputAuthority);
    }

    void OnHealthChanged()
    {
        if (!Object.HasInputAuthority) return;

        playerEvents.UILifeEvent.Invoke(CurrentHealth / MaxHealth);
    }

    #endregion
}

[System.Serializable]
public class PlayerReference
{
    [Header("armas")]
    public Transform gunAnchor;
    public GameObject GunEquiped;
    [Header("CuackShoot")]
    public Transform cuackShootRoot;
    public Bullet cuackBulletPrefab;
    [Header("Mesh")]
    public Transform meshTransform;

    public MeshRenderer meshMaterial;

}
[System.Serializable]
public class PlayerEvents
{
    public UnityEvent<float> UILifeEvent;
    public UnityEvent<float> UIShootChargeEvent;
}
