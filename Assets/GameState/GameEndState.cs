using System.Collections.Generic;
using System.Linq;
using PurrNet;
using PurrNet.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameEndState : StateNode<Dictionary<string, int>>
{
    public override void Enter(Dictionary<string, int> roundWins, bool asServer)
    {
        base.Enter(asServer);
        if(!asServer)
            return;

        var winner = roundWins.First();
        foreach (var team in roundWins)
        {
            if(team.Value > winner.Value)
                winner = team;
        }
        roundWins.Clear();
    }
}
