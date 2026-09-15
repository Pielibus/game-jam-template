using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PurrNet;
using PurrNet.Collections;
using PurrNet.StateMachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoundRunningState : StateNode<List<PlayerController>>
{
    private List<PlayerController> _players = new();
    private bool _roundEnded = false;
    public SyncVar<int> currentBid = new SyncVar<int>(0);
    public SyncVar<int> currentBidDice = new SyncVar<int>(1);
    public PlayerController playerActive;
    private BidChoice bidChoice;
    private int IndexPlayer = 0;
    private PlayerController lastPlayer;
    public List<int> Results = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    [SerializeField] private PlayerSpawningState playerSpawningState;
    [SerializeField] private RollDiceState rollDiceState;
    [SerializeField] private NumberChecker numberChecker;
    [SerializeField] private RotateDice rotateDice;
    [SerializeField] private Transform prefabExplosion;
    [SerializeField] public Dictionary<GameObject, int> currentDicesCount = new();
    public bool Running = false;
    private Transform removeDice;
    public override void Enter(List<PlayerController> data, bool asServer)
    {
        base.Enter();
        if(!asServer)
        {
            return;
        }

        Running = true;
        _players = new List<PlayerController>();
        Debug.Log(data.Count);
        foreach (var player in data)
        {
            if(player.owner.HasValue)
            {
                Debug.Log("Player Added" + player.owner.Value);
                _players.Add(player);
            }
        }
        if(!playerActive)
            playerActive = _players.First<PlayerController>();
        StartPlayerRound(playerActive.owner.Value,playerActive, true, currentBid.value, currentBidDice.value);

    }


    [TargetRpc]
    void StartPlayerRound(PlayerID playerID, PlayerController player, bool first, int bid, int bidDice)
    {
        bidChoice = GameObject.FindGameObjectWithTag("RoundView").GetComponent<BidChoice>();
        bidChoice.StartRound(player.owner.Value, player ,bid + 1, bidDice, first);
        
    }
    [ServerRpc]
    public void EndRound(PlayerController player, int bid, int bidDice)
    {
        if(player == playerActive)
        {
            currentBid.value = bid;
            currentBidDice.value = bidDice;
            
            IndexPlayer += 1;
            Debug.Log(_players.Count + " "+ IndexPlayer);
            if(_players.Count - 1 < IndexPlayer)
                IndexPlayer = 0;
            lastPlayer = playerActive;
            playerActive = _players[IndexPlayer];
            StartPlayerRound(playerActive.owner.Value,playerActive, false, bid, bidDice);
            UpdateBid(currentBid.value, currentBidDice.value);
        }
    }
    [ServerRpc]
    public void EndGame(PlayerController player)
    {
        StartCoroutine(EndGameAfterDelay(player));
    }

    private IEnumerator EndGameAfterDelay(PlayerController player)
    {
        if(player == playerActive)
        {
            Debug.Log("EndGame");
            currentDicesCount = numberChecker.dicesCount;
            float delay = 1.5f;
            foreach (var plr in _players)
            {
                if(plr.owner.HasValue)
                {
                    Anim(plr.owner.Value, plr, true);
                }
            }
            yield return new WaitForSeconds(1f);
            foreach (var dice in numberChecker.dicesCount)
            {
                if(delay >= 0.2f)
                    delay -= 0.2f;

                Results[dice.Value] += 1;
                if(dice.Value == currentBidDice.value)
                {
                    yield return new WaitForSeconds(delay);
                    HighlightDice(dice.Key.transform.parent.gameObject, true);
                    UpdateBid(Results[currentBidDice.value], currentBidDice.value);
                }
            }
            yield return new WaitForSeconds(3f);
            foreach (var dice in numberChecker.dicesCount)
            {
                HighlightDice(dice.Key.transform.parent.gameObject, false);
                UpdateBid(0, 0);
            }
            if(Results[currentBidDice.value] >= currentBid.value)
            {
                Debug.Log("NotBluffing you lost");
                loseDice(playerActive);
            }
            else
            {
                Debug.Log("Bluffing you win");
                loseDice(lastPlayer);
            }
            yield return new WaitForSeconds(5f);
            currentDicesCount.Clear();
            numberChecker.dicesCount.Clear();
            Results = new List<int>{ 0, 0, 0, 0, 0, 0, 0,};
            currentBid.value = 0;
            currentBidDice.value = 1;
            IndexPlayer += 1;
            if(_players.Count - 1 < IndexPlayer)
                IndexPlayer = 0;
            playerActive = _players[IndexPlayer];
            foreach (var plr in _players)
            {
                if(plr.owner.HasValue)
                {
                    Anim(plr.owner.Value, plr, false);
                }
            }
            if (rollDiceState == null)
            {
                Debug.LogError("RollDiceState is not assigned on RoundRunningState.");
                yield break;
            }
            yield return new WaitForSeconds(3f);
            Debug.Log(_players.Count);
            bool stateChanged = machine.SetState(rollDiceState, _players);
            Debug.Log($"Set roll dice state: {stateChanged}");
            //StartPlayerRound(playerActive.owner.Value, playerActive, true, currentBid.value, currentBidDice.value);

        }
    }
    [ObserversRpc]
    private void UpdateBid(int bid, int bidDice)
    {
        rotateDice.StartRotation(bidDice, bid);
    }
    [TargetRpc]
    private void Anim(PlayerID playerID, PlayerController player, bool start)
    {
        player.Gobelet.GetComponent<NetworkAnimator>().SetBool("Show", start);
    }
    [ObserversRpc]
    private void HighlightDice(GameObject Dice, bool on)
    {
        if(Dice && Dice.GetComponent<Outline>())
            Dice.GetComponent<Outline>().enabled = on;
    }
    
    private void loseDice(PlayerController playerController)
    {
        Debug.Log(playerController.owner.Value + "Has lost a dice and have now " + playerController.AllDice.Length);
        GameObject diceChoose = null;
        if(playerController.AllDice.Length <= 0)
        {
            losePlayer(playerController);
        }
        foreach (var dice in playerController.AllDice)
        {
            if(dice.gameObject)
            {
                diceChoose = dice;
            }
        }
        playerController.AllDice.RemoveAt(System.Array.IndexOf (playerController.AllDice, diceChoose));
        removeDice = diceChoose.transform;
        disable = false;
        
    }
    private void losePlayer(PlayerController playerController)
    {
        Explode(playerController.head.transform.position, playerController.head.transform.rotation);
        HidePlayer(playerController);
        _players.Remove(playerController);
        if(_players.Count <= 1)
        {
            machine.SetState(rollDiceState, _players);
        }

    }
    [ObserversRpc]
    private void HidePlayer(PlayerController playerController)
    {
        playerController.head.GetComponent<MeshRenderer>().enabled = false;
        playerController.GobeletPhysic.GetComponent<MeshRenderer>().enabled = false;
    }
    private bool disable = false;
    public override void StateUpdate(bool asServer)
    {
        if(!asServer || removeDice == null)
        {
            return;
        }
        if(!disable)
        {
            Rigidbody diceRigidbody = removeDice.GetComponent<Rigidbody>();
            diceRigidbody.isKinematic = true;
            diceRigidbody.detectCollisions = false;
            diceRigidbody.linearVelocity = Vector3.zero;
            diceRigidbody.angularVelocity = Vector3.zero;
            removeDice.GetComponent<DiceRoll>().enabled = false;
            disable = true;
        }
        
        if(removeDice.position.y < 4.5)
        {
            Debug.Log(removeDice.position.y);
            float speed = 0.01f;
            removeDice.position = new Vector3(removeDice.position.x, removeDice.position.y + speed, removeDice.position.z);
            removeDice.eulerAngles = new Vector3(removeDice.eulerAngles.x + speed, removeDice.eulerAngles.y + speed, removeDice.eulerAngles.z + speed);
            return;
        }
        Debug.Log(removeDice.position.y);
        Explode(removeDice.position, removeDice.rotation);
        Destroy(removeDice.gameObject);
    }
    [ObserversRpc]
    private void Explode(Vector3 position, Quaternion rotation)
    {
        var explosion = Instantiate(prefabExplosion, position, rotation);
        Transform[] childArray = explosion.GetComponentsInChildren<Transform>();
       foreach (var child in childArray)
        {
            if(child.GetComponent<ParticleSystem>())
                child.GetComponent<ParticleSystem>().Play();
        } 
    }

}
