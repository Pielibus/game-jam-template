using System.Collections;
using PurrNet.StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using PurrNet;

public class RollDiceState : StateNode<List<PlayerController>>
{
    private float time;
    public bool Stopped = false;
    [SerializeField] NumberChecker numberChecker;
    private List<PlayerController> playerControllers = new List<PlayerController>();
    public override void Enter(List<PlayerController> data, bool asServer)
    {
        base.Enter(asServer);

         if(!asServer)
            return;
        playerControllers = data;
        time = Time.fixedTime;
        Stopped = false;
        var AllDices = GameObject.FindGameObjectsWithTag("Dice");
        foreach (var dice in AllDices)
        {
            dice.GetComponent<Rigidbody>().isKinematic = false;
        }
        foreach (var player in data)
        {
            if(player.owner.HasValue)
            {
                player.center1.parent = player.GobeletPhysic;
                player.center2.parent = player.GobeletPhysic;
                player.center3.parent = player.GobeletPhysic;
                player.rolling = true;
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
        if(Time.fixedTime - time > 10 && Stopped == false)
        {
            Stopped = true;
            numberChecker.dicesCount.Clear();
            foreach (var player in playerControllers)
            {
                if(player.owner.HasValue)
                {
                    player.StopRolling(player.owner.Value);
                }
            }
        }
        if(Time.fixedTime - time > 12)
        {
            foreach (var player in playerControllers)
            {
                if(player.owner.HasValue)
                {
                    player.center1.parent = null;
                    player.center2.parent = null;
                    player.center3.parent = null;
                    player.Wall.GetComponent<MeshCollider>().enabled = true;
                    player.Close.GetComponent<BoxCollider>().enabled = false;
                    player.rolling = false;
                }
            }
            machine.Next(playerControllers);
        }
            
        
    }
   
}

