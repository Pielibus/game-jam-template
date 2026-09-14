using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PurrNet;
using PurrNet.Collections;
using PurrNet.StateMachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoundRunningState : StateNode<List<PlayerController>>
{
    private List<PlayerController> _players = new();
    private bool _roundEnded = false;
    public SyncVar<int> currentBid = new SyncVar<int>(0);
    public SyncVar<int> currentBidDice = new SyncVar<int>(1);
    public PlayerController playerActive;
    private BidChoice bidChoice;
    private int IndexPlayer;
    private PlayerController lastPlayer;
    public List<int> Results = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    [SerializeField] private PlayerSpawningState playerSpawningState;
    [SerializeField] private NumberChecker numberChecker;
    [SerializeField] private RotateDice rotateDice;
    [SerializeField] public Dictionary<GameObject, int> currentDicesCount = new();
    public bool Running = false;
    public override void Enter(List<PlayerController> data, bool asServer)
    {
        base.Enter();
        if(!asServer)
        {
            return;
        }

        Running = true;
        _players.Clear();
        foreach (var player in data)
        {
            if(player.owner.HasValue)
            {
                Debug.Log("Player Added" + player.owner.Value);
                _players.Add(player);
            }
        }
        playerActive = _players.First<PlayerController>();
        IndexPlayer = 0;
        StartPlayerRound(playerActive.owner.Value,playerActive, true, currentBid.value, currentBidDice.value);

    }

    public override void StateUpdate(bool asServer)
    {
        if(!asServer)
        {
            return;
        }
        

    }

    [TargetRpc]
    void StartPlayerRound(PlayerID playerID, PlayerController player, bool first, int bid, int bidDice)
    {
        bidChoice = GameObject.FindGameObjectWithTag("RoundView").GetComponent<BidChoice>();
        bidChoice.StartRound(player.owner.Value, player ,bid + 1, bidDice, first);
        
    }
    [ServerRpc]
    public void EndRound(PlayerController player, int bid, int bidDice)
    {
        if(player == playerActive)
        {
            currentBid.value = bid;
            currentBidDice.value = bidDice;
            
            IndexPlayer += 1;
            if(_players.Count - 1 < IndexPlayer)
                IndexPlayer = 0;
            lastPlayer = playerActive;
            playerActive = _players[IndexPlayer];
            StartPlayerRound(playerActive.owner.Value,playerActive, false, bid, bidDice);
            foreach (var plr in _players)
            {
                if(plr.owner.HasValue)
                {
                    UpdateBid(plr.owner.Value, plr, currentBid.value, currentBidDice.value);
                }
            }
        }
    }
    [ServerRpc]
    public void EndGame(PlayerController player)
    {
        if(player == playerActive)
        {
            currentDicesCount = numberChecker.dicesCount;
            foreach (var dice in numberChecker.dicesCount)
            {
                
                Results[dice.Value] += 1;
                Debug.Log(Results[dice.Value]);
            }

            if(Results[currentBidDice] >= currentBid)
            {
                Debug.Log("NotBluffing you lost");
                loseDice(playerActive);
            }
            else
            {
                Debug.Log("Bluffing you win");
                loseDice(lastPlayer);
            }
            currentDicesCount.Clear();
            numberChecker.dicesCount.Clear();
            Results = new List<int>{ 0, 0, 0, 0, 0, 0, 0,};
            currentBid.value = 0;
            currentBidDice.value = 1;
            IndexPlayer += 1;
            if(_players.Count - 1 < IndexPlayer)
                IndexPlayer = 0;
            playerActive = _players[IndexPlayer];

            
            StartPlayerRound(playerActive.owner.Value, playerActive, true, currentBid.value, currentBidDice.value);

        }
    }
    [TargetRpc]
    private void UpdateBid(PlayerID playerID, PlayerController player, int bid, int bidDice)
    {
        rotateDice.StartRotation(player, bidDice, bid);
    }
    
    private void loseDice(PlayerController playerController)
    {
        Debug.Log(playerController.owner.Value + "Has lost a dice");
        Destroy(playerController.AllDice[1].gameObject);
        playerController.AllDice.RemoveAt(1);
    }
}
