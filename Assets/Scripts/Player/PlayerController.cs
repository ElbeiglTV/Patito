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
    [Header("Life")]
    public float MaxHealth;
    public float CurrentHealth;
    #endregion

    #region Sumergirse Config
    [Header("Sumergirse Config")]
    public bool isUnderWater;
    public float waterLevel;
    public float moveBoost;
    
    TickTimer sumergirseTimer;
    public float time;
    #endregion
    #region CuackSoot


    public float multiplier = 1;
    public float multiplierMax;
    public float multiplierMin;
    public float multiplierTime;
    float multiplierTimer;
    #endregion

    #region Camera
    public MyCamera myCamera;
    public Transform RenderCameraTransformTarget;
    #endregion


    public PlayerReference playerReference;
    public PlayerEvents playerEvents;

    // Start is called before the first frame update
    public override void Spawned()
    {
        InitializePlayer();

        if (!HasStateAuthority) return;

        // NetworkGameManager.Instance.RPC_AddToList(Runner.LocalPlayer);

        GameManager.Instance.playerController = this;

        UnityEngine.Camera.main.GetComponent<MyCamera>().target = transform;
        myCamera = UnityEngine.Camera.main.GetComponent<MyCamera>();

        playerEvents.UILifeEvent.AddListener(x => GameManager.Instance.LifeFill.fillAmount = x);
        playerEvents.UILifeEvent.AddListener(x => GameManager.Instance.LifeFill2.fillAmount = x);

        playerEvents.UIShootChargeEvent.AddListener(x => GameManager.Instance.ShootFill.fillAmount = x);
        playerEvents.UIShootChargeEvent.AddListener(x => GameManager.Instance.ShootFill2.fillAmount = x);

        GameManager.Instance.RenderCamera.transform.position = RenderCameraTransformTarget.position;

        GameManager.Instance.RenderCamera.transform.LookAt(transform);

        ActualiseLifeUI();

        ActualiseShootChargeUI();


        Active = false;
        GameManager.Instance.inGameUI.SetActive(false);
        GameManager.Instance.onSpawnUI.SetActive(true);



    }

    private void Update()
    {
        if (!HasStateAuthority) return;



        if (!Active )
        {
            if (Input.GetKeyDown(KeyCode.Escape) && NetworkGameManager.Instance.GameStarted)
            {
                Active = true;
                GameManager.Instance.inGameUI.SetActive(true);
                GameManager.Instance.onSpawnUI.SetActive(false);
            }
            return;
        }
            
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
        if (multiplierTimer < multiplierTime)
        {
            multiplierTimer += Time.deltaTime;
            multiplier = multiplierMin+(multiplierMax-multiplierMin) * (multiplierTimer / multiplierTime);

            ActualiseShootChargeUI();
        }
    }
    public void CuackShootRelease()
    {
        Bullet b = Runner.Spawn(playerReference.cuackBulletPrefab, playerReference.cuackShootRoot.position, playerReference.cuackShootRoot.rotation);
        b.RPC_CuackShootBullet(multiplier/2);
        multiplier = multiplierMin;
        multiplierTimer = 0;
        ActualiseShootChargeUI();
    }

    #region Damage
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float Damage)
    {
        TakeDamage(Damage);
    }

    public void TakeDamage(float damage)
    {
        if (!Active) return;
        Debug.Log("Player Hit");
        CurrentHealth -= damage;
        myCamera.cameraShake.TriggerShake();
        ActualiseLifeUI();
        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        NetworkGameManager.Instance.RPC_Death(Runner.LocalPlayer);
        Runner.Despawn(Object);
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

    public void ActualiseLifeUI()
    {
        playerEvents.UILifeEvent.Invoke(CurrentHealth / MaxHealth);
    }
    public void ActualiseShootChargeUI()
    {
        playerEvents.UIShootChargeEvent.Invoke((multiplier-multiplierMin)/(multiplierMax-multiplierMin));
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ApplyColor(Color color)
    {
        playerReference.meshMaterial.material.color = color;
    }
    
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
