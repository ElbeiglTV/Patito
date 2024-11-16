using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LocalInputs))]
public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
    public LocalInputs Inputs { get; private set; }

    public override void Spawned()
    {
        Inputs = GetComponent<LocalInputs>();

        if (Object.HasInputAuthority)
        {
            Local = this;
            Inputs.enabled = true;
        }
        else
        {
            Inputs.enabled = false;
        }
    }
}
