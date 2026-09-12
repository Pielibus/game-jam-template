using System.Collections;
using PurrNet.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class WaitForPlayersState : StateNode
{
    [SerializeField] private int minPlayer = 1;
    private WaitForSeconds _delay = new(3f);

    public override void Enter(bool asServer)
    {
        base.Enter(asServer);

         if(!asServer)
            return;

        StartCoroutine(WaitForPlayers());
    }
    private IEnumerator WaitForPlayers()
    {
        while(networkManager.players.Count < minPlayer)
        {
            yield return null;
        }
        StartCoroutine(DelayNextState());
    }
    private IEnumerator DelayNextState()
    {
        yield return _delay;
        machine.Next();
           
    }
   
}

