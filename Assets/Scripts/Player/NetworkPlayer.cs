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
        if (HasInputAuthority)
        {
            Inputs = GetComponent<LocalInputs>();
            Local = this;
            Inputs.enabled = true;
        }
        else
        {
            GetComponent<LocalInputs>().enabled = false;
        }
    }
}
