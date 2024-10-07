using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerSpawner : SimulationBehaviour ,IPlayerJoined
{
    [SerializeField] GameObject _prefab;

    [SerializeField] Transform _spawner;
    [SerializeField] Transform _spawner2;

    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player, _prefab,_spawner);
        

    }
    public void SpawnPlayer(PlayerRef player, GameObject prefab,Transform spawner)
    {
        if (player != Runner.LocalPlayer) return;
        
        if (Runner.ActivePlayers.Count() <= 1)
        {
           Runner.Spawn(prefab,spawner.position);
        }
        else
        {
            Runner.Spawn(prefab, _spawner2.position);
        }
        

    }
}
