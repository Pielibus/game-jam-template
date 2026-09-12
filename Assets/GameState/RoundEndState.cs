using System.Collections;
using System.Collections.Generic;
using PurrNet;
using PurrNet.StateMachine;
using UnityEngine;
using UnityEngine.UIElements;

public class RoundEndState : StateNode<List<PlayerID>>
{
   [SerializeField] private int amountRound = 2;
   [SerializeField] private WaitForSeconds _delay = new(3f);
   [SerializeField] private StateNode spawningState;
   [SerializeField] private PlayerSpawningState playerSpawningState;

   private int _roundCount = 0;

   private Dictionary<string, int> _roundWins = new();

    public override void Enter(bool asServer)
    {
        base.Enter(asServer);

        if(!asServer)
            return;
        
        CheckForGameEnd();
    }


    public override void Enter(List<PlayerID> winnerList, bool asServer)
    {

        base.Enter(asServer);
        
        if(!asServer)
            return;
        
        string winningTeam = "Draw";


        if(!_roundWins.ContainsKey(winningTeam))
            _roundWins.Add(winningTeam, 0);
        _roundWins[winningTeam]++;

        CheckForGameEnd();

    }

    private void CheckForGameEnd()
    {
        _roundCount++;
        if(_roundCount >= amountRound)
        {
            machine.Next(_roundWins);
            return;
        }
        StartCoroutine(DelayNextState(_roundCount));
    }

    private IEnumerator DelayNextState(int _roundCount)
    {
        
        yield return _delay;
        machine.SetState(spawningState);
           
    }

}
