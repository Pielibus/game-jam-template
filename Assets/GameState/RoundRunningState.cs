using System;
using System.Collections.Generic;
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
    public PlayerController playerActive;
    [SerializeField] private PlayerSpawningState playerSpawningState;
    public override void Enter(List<PlayerController> data, bool asServer)
    {
        base.Enter();
        if(!asServer)
        {
            return;
        }


        _players.Clear();
        foreach (var player in data)
        {
            if(player.owner.HasValue)
            {
                _players.Add(player);
            }
        }
        playerActive = _players[1];
        StartPlayerRound(playerActive);

    }

    public override void StateUpdate(bool asServer)
    {
        if(!asServer)
        {
            return;
        }
        

    }

    void StartPlayerRound(PlayerController player)
    {
        
        
    }

    void EndRound(PlayerController player)
    {
        //Ask to Bid or liar
        
    }
}
