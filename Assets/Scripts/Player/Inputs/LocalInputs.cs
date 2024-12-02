using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LocalInputs : MonoBehaviour
{
    NetworkInputData _inputData;
    MouseRotate _rotateSystem;

    public Transform renderTarget;

    bool active;

    bool _EscPressed;
    bool _isJumpPressed;
    bool _isFirePressed;
    float _axisX;
    float _axisZ;

    Quaternion _rotation;

    Color _color;


    private void Awake()
    {
        _inputData = new NetworkInputData();
        _rotateSystem = new MouseRotate();

        

    }

    private void Start()
    {
        Camera.main.GetComponent<MyCamera>().target = transform;

        GameManager.Instance.RenderCamera.transform.position = renderTarget.position;
        GameManager.Instance.RenderCamera.transform.LookAt(transform);

        GameManager.Instance.inGameUI.SetActive(false);
        GameManager.Instance.onSpawnUI.SetActive(true);

    }
    private void Update()
    {
        _isJumpPressed |= Input.GetKeyDown(KeyCode.Space);//salto
        _isFirePressed |= Input.GetMouseButtonDown(0);//disparo
        _axisX = Input.GetAxis("Horizontal");
        _axisZ = Input.GetAxis("Vertical");
        _rotation = _rotateSystem.RotatePlayer(transform);

        _color = GameManager.Instance.PatitoColor;

        if (!active)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && NetworkGameManager.Instance.GameStarted)
            {
                active = true;
                GameManager.Instance.inGameUI.SetActive(true);
                GameManager.Instance.onSpawnUI.SetActive(false);
            }
            return;

        }

        

    }
    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.axisX = _axisX;
        _inputData.axisZ = _axisZ;

        _inputData.rotation = _rotation;
        _inputData.active = active;

        _inputData.color = _color;
        return _inputData;
    }
}
