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
            GameObject gobelet = newPlayer.transform.Find("Gobelet").gameObject;
            GameObject wall = gobelet.transform.Find("Wall").gameObject;
            gobelet.transform.parent = null;
            wall.transform.parent = null;
            currentSpawnIndex++;
        }
        
        AllPlayers = spawnedPlayers;
        return spawnedPlayers;
    }

    public override void Exit(bool asServer)
    {
        base.Exit(asServer);
    }
}

