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
        if (Input.GetMouseButtonDown(0))
        {
            _isFirePressed = true;
        }
    }
}
