using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using PurrNet;
using PurrNet.StateMachine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject[] AllDice;
    [SerializeField] public GameObject Gobelet;
    [SerializeField] private RoundRunningState roundRunningState;
    
    private NumberChecker numberChecker;
    void Awake()
    {
        numberChecker = GameObject.FindGameObjectWithTag("Ground").GetComponent<NumberChecker>();
        roundRunningState = GameObject.FindAnyObjectByType<RoundRunningState>();
    }
    public void launchRolling()
    {
        if(roundRunningState.Running)
            return;

        // //Debug.Log("Launching dices");
        // numberChecker.dicesCount.Clear();
        // foreach (var dice in AllDice)
        // {
        //     dice.GetComponent<DiceRoll>().Roll();
        // }
        Gobelet.GetComponent<RollGobelet>().StartRoll();
        
    }

    public void StartRolling()
    {
        Gobelet.GetComponent<RollGobelet>().StartRoll();
    }

    public void CheckGobelet(InputAction.CallbackContext context)
    {
            Gobelet.GetComponent<CheckGobelet>().Check(context.performed); 
          
    }
    
}
