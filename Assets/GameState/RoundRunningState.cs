using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using PurrNet;
using PurrNet.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class RoundRunningState : StateNode<List<PlayerController>>
{
    private List<PlayerID> _players = new();
    private bool _roundEnded = false;
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
                _players.Add(player.owner.Value);
            }
        }

    }

    public override void StateUpdate(bool asServer)
    {
        if(!asServer)
        {
            return;
        }

        /* if(bookCollectorBlue.NumberBook >= 3)
        {
            machine.Next(playerSpawningState.blueTeam);
            _roundEnded = true;
        }
        else if(BookCollectorRed.NumberBook >= 3)
        {
            machine.Next(playerSpawningState.redTeam);
            _roundEnded = true;
        } */

    }
}
