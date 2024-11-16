using Fusion;

public struct NetworkInputData : INetworkInput
{
    //definimos todos los inputs que vamos a utilizar
    public float movementInput;
    public NetworkBool isFirePressed;

    public NetworkButtons networkButtons;
}

enum MyButtons
{
    Jump = 0,
}
