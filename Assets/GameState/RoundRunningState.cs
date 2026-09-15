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
    private int IndexPlayer;
    private PlayerController lastPlayer;
    public List<int> Results = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    [SerializeField] private PlayerSpawningState playerSpawningState;
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
        _players.Clear();
        foreach (var player in data)
        {
            if(player.owner.HasValue)
            {
                Debug.Log("Player Added" + player.owner.Value);
                _players.Add(player);
            }
        }
        playerActive = _players.First<PlayerController>();
        IndexPlayer = 0;
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
            yield return new WaitForSeconds(100f);
            currentDicesCount.Clear();
            numberChecker.dicesCount.Clear();
            Results = new List<int>{ 0, 0, 0, 0, 0, 0, 0,};
            currentBid.value = 0;
            currentBidDice.value = 1;
            IndexPlayer += 1;
            if(_players.Count - 1 < IndexPlayer)
                IndexPlayer = 0;
            playerActive = _players[IndexPlayer];

            
            StartPlayerRound(playerActive.owner.Value, playerActive, true, currentBid.value, currentBidDice.value);

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
        Debug.Log(playerController.owner.Value + "Has lost a dice");
        GameObject diceChoose = null;
        foreach (var dice in playerController.AllDice)
        {
            if(dice.gameObject)
            {
                diceChoose = dice;
            }
        }
        playerController.AllDice.RemoveAt(System.Array.IndexOf (playerController.AllDice, diceChoose));
        removeDice = diceChoose.transform;
        
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
            removeDice.GetComponent<Rigidbody>().isKinematic = true;
            removeDice.GetComponent<Rigidbody>().detectCollisions = false;
            Destroy(removeDice.GetComponent<DiceRoll>());
            disable = true;
        }
        
        if(removeDice.position.y < 7)
        {
            Debug.Log(removeDice.position.y);
            float speed = 0.005f;
            removeDice.position = new Vector3(removeDice.position.x, removeDice.position.y + speed, removeDice.position.z);
            removeDice.eulerAngles = new Vector3(removeDice.eulerAngles.x + speed, removeDice.eulerAngles.y + speed, removeDice.eulerAngles.z + speed);
            return;
        }
        Debug.Log(removeDice.position.y);
        var explosion = Instantiate(prefabExplosion, removeDice.position, removeDice.rotation);
        ParticleSystem[] childArray = explosion.GetComponentsInChildren<ParticleSystem>();
        foreach (var child in childArray)
        {
            child.Play();
        }
        Destroy(removeDice);
        

        

    }

}
