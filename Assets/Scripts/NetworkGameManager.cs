using Fusion;
using UnityEngine;
using System.Linq;

public class NetworkGameManager : NetworkBehaviour
{
    public static NetworkGameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Transform spawnpoint1;
    public Transform spawnpoint2;

    public bool GameStarted => Runner.ActivePlayers.Count() > 1;

    [Networked, OnChangedRender(nameof(OnGameEndedChanged))]
    public NetworkBool GameEnded { get; set; }

    

    [Networked]
    public PlayerRef Loser { get; set; }

    public GameObject WIN;
    public GameObject LOSE;

    public void RegisterPlayerDeath(PlayerRef deadPlayer)
    {
        if (!HasStateAuthority) return;
        if (GameEnded) return;

        Loser = deadPlayer;
        GameEnded = true;
        OnGameEndedChanged();
    }

    void OnGameEndedChanged()
    {
        if (!GameEnded) return;

        bool iLost = Runner.LocalPlayer == Loser;

        if (iLost)
            LOSE.SetActive(true);
        else
            WIN.SetActive(true);
    }
}