using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInputs : MonoBehaviour
{
    NetworkInputData _inputData;
    bool _isJumpPressed;
    bool _isFirePressed;

    private void Awake()
    {
        _inputData = new NetworkInputData();

    }
    private void Update()
    {
        _inputData.axisX = Input.GetAxis("Horizontal");
        _inputData.axisZ = Input.GetAxis("Forward");

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }*/
        _isJumpPressed |= Input.GetKeyDown(KeyCode.Space);//salto
        _isFirePressed |= Input.GetMouseButtonDown(0);
    }
    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        return _inputData;
    }
}
