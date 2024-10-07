using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkGameManager : NetworkBehaviour
{
    public static NetworkGameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
        Instance = this;
        }
        else 
        {
            Destroy(this);
            return;
        }
    }

    public List<PlayerRef> PlayerList = new List<PlayerRef>();

    public bool GameStarted => Runner.ActivePlayers.Count() > 1;


    public GameObject WIN;
    public GameObject LOSE;
    

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void RPC_Death(PlayerRef pRef)
    {
       if(pRef == Runner.LocalPlayer)
        {
            LOSE.SetActive(true);
        }
       else
        {
            WIN.SetActive(true);
        }
    }

}
