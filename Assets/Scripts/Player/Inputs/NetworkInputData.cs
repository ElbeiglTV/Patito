using Fusion;
using UnityEngine;

public struct NetworkInputData: INetworkInput
{
    public float axisX;
    public float axisZ;

    public NetworkBool isFirePressed;
    public NetworkBool isJumpPressed;

}
