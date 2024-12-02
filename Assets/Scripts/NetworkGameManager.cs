using Fusion;
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

    public Transform spawnpoint1;
    public Transform spawnpoint2;

    [Networked] public bool Player1Lose { get; set; }
    [Networked] public bool Player2Lose { get; set; } 

    public GameObject WIN;
    public GameObject LOSE;

    


    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_Death(PlayerRef pRef)
    {
        if (pRef == Runner.LocalPlayer)
        {
            LOSE.SetActive(true);
        }
        else
        {
            WIN.SetActive(true);
        }
    }

}
