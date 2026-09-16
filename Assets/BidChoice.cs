using System;
using PurrNet;
using TMPro;
using Unity.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BidChoice : NetworkBehaviour
{
    private int bid = 1;
    private int bidDice = 1;
    [SerializeField] private RoundRunningState roundRunningState;
    [SerializeField] private TextMeshPro Bid;
    [SerializeField] private TextMeshPro ShowBid;
    [SerializeField] private TextMeshPro Bluff;
    [SerializeField] private TextMeshPro Minus;
    [SerializeField] private TextMeshPro Plus;
    [SerializeField] private TextMeshPro Confirm;
    [SerializeField] private GameObject Dice;
    [SerializeField] private Transform ParentDice;
    [SerializeField] public Dictionary<int, Vector3> faces = new();
    [SerializeField] public float speed;
    [SerializeField] public float maxTime = 10;
    [SerializeField] public float minTime = 5;
    public Vector3 targetAngle;
    private bool on = false;
    [SerializeField] private NetworkAudioSource audioSource;

    private Vector3 currentAngle;
    private PlayerController player;
    public int CurrentBid, CurrentBidDice;
    private PlayerController mainController;
    private float time;
    private float randomTime;

    public void ListenClick(string button)
    {
        if(button == "Bid")
        {
            if (Bid.enabled)
            {
                OpenBid();
            }
            
        }
        if(button == "Bluff")
        {
            if (Bluff.enabled)
            {
                on = false;
                roundRunningState.EndGame(mainController);
                CloseBid();
            }
            
        }
        if(button == "Minus")
        {
            if (Minus.enabled)
            {
                if(bid > CurrentBid)
                    bid -= 1;
            }
            
        }
        if(button == "Plus")
        {
            if (Plus.enabled)
            {
                bid += 1;
            }
        }
        if(button == "Dice")
        {
            if(bidDice >= 6)
            {
                bidDice = 1;
            }
            else
            {
                bidDice += 1;
            }
            StartRotation(mainController, bidDice);
        }
        if(button == "Confirm")
        {
            if(Confirm.enabled)
            {
                on = false;
                CloseBid();
                roundRunningState.EndRound(mainController, bid, bidDice);
            }
                
        }
        UpdateSound();
        UpdateUI(bid);

    }
    [ObserversRpc]
    public void StartRotation(PlayerController plr, int bidDice)
    {
        player = plr;
        currentAngle = Dice.transform.localEulerAngles;
        
        targetAngle = faces[bidDice];
        time = Time.fixedTime;
        randomTime = Random.Range(minTime, maxTime);
        on = true;

    }
    public void Update()
    {
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * speed),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * speed),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * speed));

        Dice.transform.localEulerAngles = currentAngle;
        if(Time.fixedTime - time > randomTime && on)
            Hmm();
    }
    private void Hmm()
    {
        time = Time.fixedTime;
        randomTime = Random.Range(minTime, maxTime);
        player.GetComponent<AudioSource>().Play();
    }
    [ObserversRpc]
    public void StartRound(PlayerID playerID, PlayerController playerController, int currentBid, int currentBidDice, bool first)
    {
        mainController = playerController;
        Open(first, mainController);
        CurrentBid = currentBid;
        CurrentBidDice = currentBidDice;
        bid = currentBid;
        bidDice = currentBidDice;

    }
    [ObserversRpc]
    private void UpdateUI(int bidSend)
    {
       ShowBid.text = bidSend.ToString(); 
    }
    [ServerRpc]
    private void UpdateSound()
    {
       audioSource.Play();
    }

    private void Open(bool first, PlayerController mainController)
    {
        Bid.GetComponent<BoxCollider>().enabled = true;
        Bid.gameObject.SetActive(true);
        Bluff.gameObject.SetActive(true);
        Bluff.enabled = true;
        Bid.enabled = true;
        Minus.enabled = false;
        Plus.enabled = false;
        Confirm.enabled = false;
        ShowBid.enabled = false;
        Dice.SetActive(false);
        Debug.Log(mainController.spawnerBid.value);
        transform.position = mainController.spawnerBid.value;
        transform.eulerAngles = mainController.spawnerBidRotation.value;
        StartRotation(mainController, bidDice);
        
        
        if(first)
        {
            Bluff.enabled = false;
        }
        else  
        {
            Bluff.enabled = true;
        }
    }
    [ObserversRpc]
    private void OpenBid()
    {
        Bid.enabled = false;
        Bid.GetComponent<BoxCollider>().enabled = false;
        Minus.enabled = true;
        Plus.enabled = true;
        ShowBid.enabled = true;
        Confirm.enabled = true;
        Dice.SetActive(true);
        StartRotation(mainController, bidDice);
    }
    [ObserversRpc]
    private void CloseBid()
    {
        Bid.gameObject.SetActive(false);
        Bluff.gameObject.SetActive(false);
        Minus.enabled = false;
        Plus.enabled = false;
        Confirm.enabled = false;
        ShowBid.enabled = false;
        Dice.SetActive(false);
    }
}
