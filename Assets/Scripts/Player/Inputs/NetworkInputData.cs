using Fusion;
using UnityEngine;

public struct NetworkInputData: INetworkInput
{
    public float axisX;
    public float axisZ;

    public Quaternion rotation;

    public NetworkBool isFirePressed;
    public NetworkBool isJumpPressed;

    public NetworkBool active;

    public Color color;
}
