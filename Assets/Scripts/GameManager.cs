using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #region INGAME UI

    public GameObject inGameUI;

    public Image LifeFill;
    public Image LifeFill2;

    public Image ShootFill;
    public Image ShootFill2;
    #endregion

    #region OnSpawned
    public GameObject onSpawnUI;

    public Camera RenderCamera;

    public Slider Rslider;
    public Slider Gslider;
    public Slider Bslider;
    public Image colorPreview;
    public Color PatitoColor;

   public bool player1Lose;
    public bool player2Lose;

    public NewPlayerController playerController;
    #endregion


    public void ActualizeSliderR(float amount)
    {
        Rslider.value = amount;
        PatitoColor.r = amount;
        PatitoColor.a = 1;
        colorPreview.color = PatitoColor;
        Rslider.image.color = new Color(amount, 0, 0);
        
    }
    public void ActualizeSliderG(float amount)
    {
        Gslider.value = amount;
        PatitoColor.g = amount;
        PatitoColor.a = 1;
        colorPreview.color = PatitoColor;

        Gslider.image.color = new Color(0, amount, 0);
        
    }
    public void ActualizeSliderB(float amount)
    {
        Bslider.value = amount;
        PatitoColor.b = amount;
        PatitoColor.a = 1;
        colorPreview.color = PatitoColor;
        Bslider.image.color = new Color(0, 0, amount);
        
    }
    private void Update()
    {
        player1Lose = NetworkGameManager.Instance.Player1Lose;
        player2Lose = NetworkGameManager.Instance.Player2Lose;


        if (player1Lose && !player2Lose)
        {
            if (NetworkGameManager.Instance.HasStateAuthority)
            {
                NetworkGameManager.Instance.LOSE.SetActive(true);
            }
            else
            {
                NetworkGameManager.Instance.WIN.SetActive(true);
            }
        }
        else if (player2Lose && !player1Lose)
        {
            if (NetworkGameManager.Instance.HasInputAuthority)
            {
                NetworkGameManager.Instance.WIN.SetActive(true);
            }
            if (!NetworkGameManager.Instance.HasStateAuthority)
            {
                NetworkGameManager.Instance.LOSE.SetActive(true);
            }
        }
    }



}
