using Fusion;
using UnityEngine;

public struct NetworkInputData: INetworkInput
{
    public float axisX;
    public float axisZ;

    public Vector3 MousePosition;

    public NetworkBool isFirePressed;
    public NetworkBool isJumpPressed;

}
