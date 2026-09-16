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
    [SerializeField] public GameObject head;
    [SerializeField] public Transform center1;
    [SerializeField] public Transform center2;
    [SerializeField] public Transform center3;
    [SerializeField] public Transform GobeletPhysic;
    [SerializeField] public GameObject Wall;
    [SerializeField] public GameObject Close;
    private RoundRunningState roundRunningState;
    
    private NumberChecker numberChecker;
    public Vector3 OGRotation;
    public SyncVar<Vector3> spawnerBid = new SyncVar<Vector3>(Vector3.zero, ownerAuth:false);
    public SyncVar<Vector3> spawnerBidRotation = new SyncVar<Vector3>(Vector3.zero, ownerAuth:false);
    void Awake()
    {
        numberChecker = GameObject.FindGameObjectWithTag("Ground").GetComponent<NumberChecker>();
        roundRunningState = GameObject.FindAnyObjectByType<RoundRunningState>();
    }
    void Start()
    {
        if(isServer)
        {
            foreach (var dice in AllDice)
            {
                dice.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
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
        if(!isOwner)
            return;

        Gobelet.GetComponent<CheckGobelet>().Check(owner.Value, context.performed); 
    }
    
}
