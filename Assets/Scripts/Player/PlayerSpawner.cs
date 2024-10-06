using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour ,IPlayerJoined
{
    [SerializeField] GameObject _prefab;

    [SerializeField] Transform _spawner; 

    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player, _prefab,_spawner);
        

    }
    public void SpawnPlayer(PlayerRef player, GameObject prefab,Transform spawner)
    {
        if (player != Runner.LocalPlayer) return;
        Runner.Spawn(prefab,spawner.position);
    }
}
