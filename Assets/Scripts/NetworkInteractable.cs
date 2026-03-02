using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class NetworkInteractable : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnStateChanged))]
    public NetworkBool IsActive { get; set; }

    public GameObject visualTarget; // puerta, luz, etc

    public UnityEvent OnState;

    public void TryInteract(PlayerRef player)
    {
        if (!HasStateAuthority) return;
        Debug.Log("[Interact] TryInteract NetworkOBJINteract");

        // Cambiamos estado
        IsActive = !IsActive;
    }

    void OnStateChanged()
    {
        Debug.Log("[Interact] OnStateChange NetworkInteract");
        OnState?.Invoke();
        visualTarget.SetActive(IsActive);
    }
}