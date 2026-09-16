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
    private Vector3 center1LocalPosition;
    private Vector3 center2LocalPosition;
    private Vector3 center3LocalPosition;
    private Quaternion center1LocalRotation;
    private Quaternion center2LocalRotation;
    private Quaternion center3LocalRotation;
    void Awake()
    {
        numberChecker = GameObject.FindGameObjectWithTag("Ground").GetComponent<NumberChecker>();
        roundRunningState = GameObject.FindAnyObjectByType<RoundRunningState>();
        center1LocalPosition = GobeletPhysic.InverseTransformPoint(center1.position);
        center2LocalPosition = GobeletPhysic.InverseTransformPoint(center2.position);
        center3LocalPosition = GobeletPhysic.InverseTransformPoint(center3.position);
        center1LocalRotation = Quaternion.Inverse(GobeletPhysic.rotation) * center1.rotation;
        center2LocalRotation = Quaternion.Inverse(GobeletPhysic.rotation) * center2.rotation;
        center3LocalRotation = Quaternion.Inverse(GobeletPhysic.rotation) * center3.rotation;
    }

    public void AttachDiceCenters()
    {
        AttachDiceCenter(center1, center1LocalPosition, center1LocalRotation);
        AttachDiceCenter(center2, center2LocalPosition, center2LocalRotation);
        AttachDiceCenter(center3, center3LocalPosition, center3LocalRotation);
    }

    private void AttachDiceCenter(Transform center, Vector3 localPosition, Quaternion localRotation)
    {
        center.SetParent(GobeletPhysic, false);
        center.localPosition = localPosition;
        center.localRotation = localRotation;
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
