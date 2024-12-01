using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInputs : MonoBehaviour
{
    NetworkInputData _inputData;
    bool _isJumpPressed;
    bool _isFirePressed;
    float _axisX;
    float _axisZ;

    private void Awake()
    {
        _inputData = new NetworkInputData();

    }
    private void Update()
    {
        _isJumpPressed |= Input.GetKeyDown(KeyCode.Space);//salto
        _isFirePressed |= Input.GetMouseButtonDown(0);//disparo
        _axisX = Input.GetAxis("Horizontal");
        _axisZ = Input.GetAxis("Vertical");

    }
    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.axisX = _axisX;
        _inputData.axisZ = _axisZ;
        return _inputData;
    }
}
