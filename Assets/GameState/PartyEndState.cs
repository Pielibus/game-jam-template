using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PurrNet;
using PurrNet.StateMachine;
using UnityEngine;
using UnityEngine.UIElements;

public class PartyEndState : StateNode<List<PlayerController>>
{
    public override void Enter(List<PlayerController> _players, bool asServer)
    {
        base.Enter(asServer);
        if(!asServer)
            return;

        var winner = _players.First();
        Debug.Log("Winner is "+ winner.owner.Value);
    }

}
