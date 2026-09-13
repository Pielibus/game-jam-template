using System.Collections;
using PurrNet.StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using PurrNet;

public class RollDiceState : StateNode<List<PlayerController>>
{
    public override void Enter(List<PlayerController> data, bool asServer)
    {
        base.Enter(asServer);

         if(!asServer)
            return;
        foreach (var player in data)
        {
            if(player.owner.HasValue)
            {
                player.StartRolling();
            }
        }

    }
   
}

