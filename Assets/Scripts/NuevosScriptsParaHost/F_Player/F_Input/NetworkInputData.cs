using Fusion;
using System.Numerics;

public struct NetworkInputData : INetworkInput
{
    //definimos todos los inputs que vamos a utilizar

    public NetworkBool isUnderWater;
    public NetworkBool ActiveUI;
    public Vector3 movementInput;


    public NetworkBool isFirePressed;
    public NetworkButtons networkButtons;
}

enum MyButtons
{
    Jump = 0,
}
