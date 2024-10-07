using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    #region Movement
    private Vector3 input;
    private Vector3 MoveVector;

    public float Speed;
    public float Gravity = 9.81f;

    MouseRotate RotateSystem;
    CharacterController characterController;
    #endregion

    #region Life
    [Header("Life")]
    public float MaxHealth;
    public float CurrentHealth;
    #endregion

    [Header("Sumergirse Config")]
    public bool isUnderWater;
    public float waterLevel;
    public float moveBoost;
    
    TickTimer sumergirseTimer;
    public float time;

    #region CuackSoot

    public float multiplier;

    #endregion

    #region Camera
    public MyCamera myCamera;
    #endregion


    public PlayerReference playerReference;

    // Start is called before the first frame update
    public override void Spawned()
    {
        InitializePlayer();

        if (!HasStateAuthority) return;

        UnityEngine.Camera.main.GetComponent<MyCamera>().target = transform;
        myCamera = UnityEngine.Camera.main.GetComponent<MyCamera>();
    }

    private void Update()
    {
        if (!HasStateAuthority) return;

        InputUpdater();
        if (Input.GetKey(KeyCode.Mouse0) && !isUnderWater)
        {
            CuackShoot();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0)&& !isUnderWater)
        {
            CuackShootRelease();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isUnderWater) return;
            RPC_ToggleSumergirse();
        }
        

    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        SumergirseCheck();
        SetMoveVector();
        SetGravity();
        RotateSystem.RotatePlayer(transform);
        characterController.Move(MoveVector * Runner.DeltaTime);
    }
    private void SetGravity()
    {
        if (characterController.isGrounded)
        {
            MoveVector.y = -Gravity * Runner.DeltaTime;
        }
        else
        {
            MoveVector.y -= Gravity * Runner.DeltaTime;
        }
    }
    private void SetMoveVector()
    {

        MoveVector = new Vector3(input.x, MoveVector.y / Speed, input.z);

        if(!isUnderWater)MoveVector = MoveVector * Speed;
        else MoveVector = MoveVector * Speed * moveBoost;
    }

    private void InputUpdater()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;


    }
    public void InitializePlayer()
    {
        RotateSystem = new MouseRotate();
        characterController = GetComponent<CharacterController>();
    }

    public void CuackShoot()
    {
        if (multiplier < 5)
        {
           multiplier = multiplier += Time.deltaTime;
            
        }
        
    }
    public void CuackShootRelease()
    {
        Bullet b = Runner.Spawn(playerReference.cuackBulletPrefab, playerReference.cuackShootRoot.position, playerReference.cuackShootRoot.rotation);
        b.RPC_CuackShootBullet(multiplier/2);
        multiplier = 1f; 
    }

    #region Damage
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float Damage)
    {
        TakeDamage(Damage);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player Hit");
        CurrentHealth -= damage;
        myCamera.cameraShake.TriggerShake();
        if (CurrentHealth <= 0)
        {
            Runner.Despawn(Object);
        }
    }
    #endregion

    #region Sumergirse  en el agua
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ToggleSumergirse()
    {
        TogleSumergirse();
    }

    public void TogleSumergirse()
    {
        if (isUnderWater)
        {
            playerReference.meshTransform.position = new Vector3(playerReference.meshTransform.position.x, playerReference.meshTransform.position.y + waterLevel, playerReference.meshTransform.position.z);
            isUnderWater = false;
        }
        else
        {
            
            playerReference.meshTransform.position = new Vector3(playerReference.meshTransform.position.x, playerReference.meshTransform.position.y - waterLevel, playerReference.meshTransform.position.z);
            sumergirseTimer = TickTimer.CreateFromSeconds(Runner, time);
            isUnderWater = true;
        }
    }
    public void SumergirseCheck()
    {
        if (!isUnderWater) return;
        if (!sumergirseTimer.Expired(Runner)) return;
        RPC_ToggleSumergirse();


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

}
