using UnityEngine;

public class DragRotateY : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 0.4f;   // sensibilidad
    [SerializeField] private float inertiaDamping = 5f;    // frenado

    private bool _isDragging;
    private float _lastMouseX;
    private float _currentVelocity; // grados por segundo

    void Update()
    {
        HandleInput();
        ApplyInertia();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverObject())
            {
                _isDragging = true;
                _lastMouseX = Input.mousePosition.x;
                _currentVelocity = 0f; // reseteamos velocidad previa
            }
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            float currentMouseX = Input.mousePosition.x;
            float delta = currentMouseX - _lastMouseX;

            float rotationAmount = delta * rotationSpeed;

            transform.Rotate(Vector3.up, rotationAmount, Space.World);

            // guardamos velocidad para la inercia
            _currentVelocity = rotationAmount / Time.deltaTime;

            _lastMouseX = currentMouseX;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    private void ApplyInertia()
    {
        if (_isDragging)
            return;

        if (Mathf.Abs(_currentVelocity) > 0.01f)
        {
            transform.Rotate(Vector3.up, _currentVelocity * Time.deltaTime, Space.World);

            // desaceleración progresiva
            _currentVelocity = Mathf.Lerp(_currentVelocity, 0f, inertiaDamping * Time.deltaTime);
        }
        else
        {
            _currentVelocity = 0f;
        }
    }

    private bool IsPointerOverObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform == transform;
        }

        return false;
    }
}