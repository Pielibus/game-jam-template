using System.Collections.Generic;
using PurrNet;
using PurrNet.StateMachine;
using UnityEngine;

public class PlayerSpawningState : StateNode
{
    [SerializeField] private Dictionary<PlayerID, GameObject> playerCharacters = new();
    [SerializeField] private PlayerController prefabPlayer;

    public List<PlayerController> AllPlayers = new List<PlayerController>();
    public List<Transform> spawnPoints = new List<Transform>();
    public override void Enter(bool asServer)
    {
        base.Enter(asServer);
        if (!asServer)
            return;

        DespawnPlayers();
        var spawnedPlayers = SpawnPlayers();

        machine.Next(spawnedPlayers);
    }
    private void DespawnPlayers()
    {
       var allPlayers = FindObjectsByType<PlayerController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
       foreach (var player in allPlayers)
       {
            Destroy(player.gameObject);
        }
    }

    private List<PlayerController> SpawnPlayers()
    {
        int currentSpawnIndex = 0;

        var spawnedPlayers = new List<PlayerController>();
        AllPlayers.Clear();
        foreach (var player in networkManager.players)
        {
            var spawnPoint = spawnPoints[currentSpawnIndex];
            var newPlayer = Instantiate(prefabPlayer, spawnPoint.position, spawnPoint.rotation);
            newPlayer.GiveOwnership(player);
            Debug.Log(player + " " + newPlayer + " " + newPlayer.gameObject);
            playerCharacters[player] = newPlayer.gameObject;
            spawnedPlayers.Add(newPlayer);
            Debug.Log(playerCharacters + " " + playerCharacters[player]);
            currentSpawnIndex++;
        }
        
        SetClients(playerCharacters);
        AllPlayers = spawnedPlayers;
        return spawnedPlayers;
    }

    public override void Exit(bool asServer)
    {
        base.Exit(asServer);
    }


    [ObserversRpc(bufferLast:true)]
    private void SetClients(Dictionary<PlayerID, GameObject> _playerCharacters)
    {
        foreach (var kvp in _playerCharacters)
        {
            Debug.Log(kvp);
            Debug.Log(kvp.Value);
            Debug.Log(kvp.Value.GetComponent<PlayerController>());
            var newPlayer = kvp.Value.GetComponent<PlayerController>();
            var playerName = "Player " + newPlayer.owner.Value;
            GameObject gobelet = newPlayer.transform.Find("Gobelet").gameObject;
            gobelet.transform.parent = null;
            gobelet.name = "Gobelet " + newPlayer.owner.Value;
            kvp.Value.name = playerName;

        }
    }
}

