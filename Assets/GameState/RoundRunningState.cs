using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PurrNet;
using PurrNet.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class RoundRunningState : StateNode<List<PlayerController>>
{
    private List<PlayerController> _players = new();
    private bool _roundEnded = false;
    public int Number1 = 0;
    public int Number2 = 0;
    public int Number3 = 0;
    public int Number4 = 0;
    public int Number5 = 0;
    public int Number6 = 0;
    public int currentBid = 1;
    public int currentBidDice = 1;
    public PlayerController playerActive;
    private BidChoice bidChoice;
    private int IndexPlayer;
    private PlayerController lastPlayer;
    public List<int> Results = new List<int>{ 0, 0, 0, 0, 0, 0 };
    [SerializeField] private PlayerSpawningState playerSpawningState;
    [SerializeField] private NumberChecker numberChecker;
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
        IndexPlayer = 1;
        StartPlayerRound(playerActive.owner.Value,playerActive, true);

    }

    public override void StateUpdate(bool asServer)
    {
        if(!asServer)
        {
            return;
        }
        

    }

    [TargetRpc]
    void StartPlayerRound(PlayerID playerID, PlayerController player, bool first)
    {
        Debug.Log(player.owner.Value + " Has start Round");
        bidChoice = GameObject.FindGameObjectWithTag("RoundView").GetComponent<BidChoice>();
        bidChoice.StartRound(player, currentBid + 1, currentBidDice, first);
        
    }

    public void EndRound(PlayerController player, int bid, int bidDice)
    {
        if(player == playerActive)
        {
            currentBid = bid;
            currentBidDice = bidDice;
            
            IndexPlayer += 1;
            if(_players.Count - 1 < IndexPlayer)
                IndexPlayer = 0;
            Debug.Log(IndexPlayer + " "+ _players.Count);
            lastPlayer = playerActive;
            playerActive = _players[IndexPlayer];
            StartPlayerRound(playerActive.owner.Value,playerActive, false);
            Debug.Log(player.owner.Value + " Has End Round");
        }
    }
    public void EndGame(PlayerController player)
    {
        if(player == playerActive)
        {
            currentDicesCount = numberChecker.dicesCount;
            foreach (var dice in numberChecker.dicesCount)
            {
                Debug.Log(dice.Value);
                Debug.Log(Results[dice.Value]);
                Results[dice.Value] += 1;
            }

            if(Results[currentBidDice] >= currentBid)
            {
                loseDice(playerActive);
            }
            else
            {
                loseDice(lastPlayer);
            }
            currentDicesCount.Clear();
            numberChecker.dicesCount.Clear();
            Results.Clear();
            currentBid =0;
            currentBidDice = 1;
            IndexPlayer += 1;
            if(_players.Count - 1 < IndexPlayer)
                IndexPlayer = 0;

            
            StartPlayerRound(playerActive.owner.Value, playerActive, true);

        }
    }
    
    private void loseDice(PlayerController playerController)
    {
        Debug.Log(playerController.owner.Value + "Has lost a dice");
        Destroy(playerController.AllDice[1].gameObject);
    }
}
