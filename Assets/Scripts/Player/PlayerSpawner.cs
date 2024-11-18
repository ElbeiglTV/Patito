using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using Fusion.Sockets;
using System.Linq;

public class PlayerSpawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    //[SerializeField] GameObject _prefab;
    [SerializeField] NetworkPrefabRef _playerRef;

    [SerializeField] Transform _spawner;
    [SerializeField] Transform _spawner2;

    /*public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player, _prefab,_spawner);
    }*/
    /*public void SpawnPlayer(PlayerRef player, GameObject prefab, Transform spawner)
    {
        if (player != Runner.LocalPlayer) return;

        if (Runner.ActivePlayers.Count() <= 1)
        {
            Runner.Spawn(prefab, spawner.position);
        }
        else
        {
            Runner.Spawn(prefab, _spawner2.position);
        }
    }*/
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Runner.Spawn(_playerRef, _spawner.position, null, player);
        }
        else
        {
            runner.Spawn(_playerRef, _spawner2.position, null, player);
        }
    }

    #region Unused Callbacks
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion
}
