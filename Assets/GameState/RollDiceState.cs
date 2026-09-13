using System.Collections;
using PurrNet.StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using PurrNet;

public class RollDiceState : StateNode<List<PlayerController>>
{
    private float time;
    private bool Stopped = false;
    private List<PlayerController> playerControllers = new List<PlayerController>();
    public override void Enter(List<PlayerController> data, bool asServer)
    {
        base.Enter(asServer);

         if(!asServer)
            return;
        playerControllers = data;
        time = Time.fixedTime;
        foreach (var player in data)
        {
            if(player.owner.HasValue)
            {
                player.StartRolling(player.owner.Value);
            }
        }

    }
    public override void StateUpdate(bool asServer)
    {
        if(!asServer)
        {
            return;
        }
        if(Time.fixedTime - time > 5 && Stopped == false)
        {
            Stopped = true;
            foreach (var player in playerControllers)
            {
                if(player.owner.HasValue)
                {
                    player.StopRolling(player.owner.Value);
                }
            }
        }
        if(Time.fixedTime - time > 10)
            machine.Next(playerControllers);
        
    }
   
}

