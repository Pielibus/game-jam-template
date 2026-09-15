using System.Collections.Generic;
using PurrNet;
using PurrNet.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawningState : StateNode
{
    [SerializeField] private Dictionary<PlayerID, GameObject> playerCharacters = new();
    [SerializeField] private PlayerController prefabPlayer;
    [SerializeField] private RotateDice rotateDice;

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
            newPlayer.spawnerBid.value = spawnPoint.Find("BidSpawn").position;
            newPlayer.spawnerBidRotation.value = spawnPoint.Find("BidSpawn").eulerAngles;
            newPlayer.GiveOwnership(player, propagateToChildren: true);
            playerCharacters[player] = newPlayer.gameObject;
            spawnedPlayers.Add(newPlayer);
            GameObject gobelet = newPlayer.transform.Find("Gobelet").gameObject;
            GameObject wall = gobelet.transform.Find("Wall").gameObject;
            gobelet.transform.parent = null;
            wall.transform.parent = null;
            SetClient(player, newPlayer);
            newPlayer.OGRotation = spawnPoint.rotation.eulerAngles;
            currentSpawnIndex++;
        }
        
        AllPlayers = spawnedPlayers;
        return spawnedPlayers;
    }

    [TargetRpc]
    private void SetClient(PlayerID playerID, PlayerController player)
    {
        var Cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        var allInputs = FindObjectsByType<PlayerInput>();
        foreach (GameObject camera in Cameras)
        {
            camera.SetActive(false);
        }
        foreach (var input in allInputs)
        {
            if (input != player.GetComponent<PlayerInput>())
                input.enabled = false;
        }
        var localInput = player.GetComponent<PlayerInput>();
        localInput.enabled = true;
        player.head.GetComponent<MeshRenderer>().enabled = false;
        player.head.transform.parent.Find("Main Camera").gameObject.SetActive(true);
        rotateDice.player = player;
    }
    public override void Exit(bool asServer)
    {
        base.Exit(asServer);
    }
}

