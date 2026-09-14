using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using PurrNet;
using PurrNet.StateMachine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] public GameObject[] AllDice;
    [SerializeField] public GameObject Gobelet;
    [SerializeField] public GameObject nose;
    [SerializeField] public Transform center;
    [SerializeField] public Transform GobeletPhysic;
    [SerializeField] public GameObject Wall;
    [SerializeField] public GameObject Close;
    private RoundRunningState roundRunningState;
    
    private NumberChecker numberChecker;
    void Awake()
    {
        numberChecker = GameObject.FindGameObjectWithTag("Ground").GetComponent<NumberChecker>();
        roundRunningState = GameObject.FindAnyObjectByType<RoundRunningState>();
    }
    [TargetRpc]
    public void StartRolling(PlayerID playerID)
    {
        Gobelet.GetComponent<RollGobelet>().StartRoll();
    }
    [TargetRpc]
    public void StopRolling(PlayerID playerID)
    {
        Gobelet.GetComponent<RollGobelet>().StopRoll();
    }

    public void CheckGobelet(InputAction.CallbackContext context)
    {
            Gobelet.GetComponent<CheckGobelet>().Check(context.performed); 
          
    }
    
}
