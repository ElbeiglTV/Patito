using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyCamera : NetworkBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public CameraShake cameraShake;

    private void Start()
    {
        cameraShake.Initialize(transform,this);
    }
    void LateUpdate()
    {
        if (target == null)
            return;
        if (!HasStateAuthority) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
[System.Serializable]
public class CameraShake
{
    private MyCamera cam;
    private Transform transform;
    // Duración del shake
    public float shakeDuration = 0.5f;

    // Magnitud del shake (intensidad)
    public float shakeMagnitude = 0.2f;

    // Frecuencia del shake
    public float dampingSpeed = 1.0f;

    // Posición original de la cámara
    private Vector3 initialPosition;

    public void Initialize(Transform camTransform,MyCamera camera)
    {
        transform = camTransform;
        initialPosition = camTransform.localPosition;
        cam = camera;
    }

    public void TriggerShake(float duration = -1f)
    {
        if (duration > 0)
        {
            shakeDuration = duration;
        }
        cam.StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Desplazamiento aleatorio dentro de un rango
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = initialPosition + randomOffset;

            // Incrementar tiempo
            elapsedTime += Time.deltaTime;

            // Atenuar el shake con el tiempo
            shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, elapsedTime / shakeDuration);

            yield return null; // Esperar un frame
        }

        // Restaurar la posición original
        transform.localPosition = initialPosition;
    }
}
