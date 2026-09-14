using System;
using PurrNet;
using TMPro;
using Unity.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] public Dictionary<int, Vector3> faces = new();
    public Vector3 targetAngle;

    private Vector3 currentAngle;
    private PlayerController player;
    public int CurrentBid, CurrentBidDice;
    private PlayerController mainController;

    public void ListenClick(string button)
    {
        if(button == "Bid")
        {
            if (Bid.enabled)
            {
                Bid.enabled = false;
                Minus.enabled = true;
                Plus.enabled = true;
                ShowBid.enabled = true;
                Confirm.enabled = true;
                Dice.SetActive(true);
            }
            
        }
        if(button == "Bluff")
        {
            if (Bluff.enabled)
            {
                roundRunningState.EndGame(mainController);
                gameObject.SetActive(false);
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
            StartRotation(mainController, bidDice, bid);
        }
        if(button == "Confirm")
        {
            if(Confirm.enabled)
            {
                roundRunningState.EndRound(mainController, bid, bidDice);
                gameObject.SetActive(false);
            }
                
        }
        ShowBid.text = bid.ToString();
    }
    public void StartRotation(PlayerController plr, int bidDice, int bid)
    {
        player = plr;
        currentAngle = Dice.transform.eulerAngles;
        targetAngle = faces[bidDice];
        targetAngle.y += player.OGRotation.y;
    }
    public void Update()
    {
        if(!player)
            return;

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

        Dice.transform.eulerAngles = currentAngle;
    }
    [TargetRpc]
    public void StartRound(PlayerID playerID, PlayerController playerController, int currentBid, int currentBidDice, bool first)
    {

        Bluff.enabled = true;
        Bid.enabled = true;
        Minus.enabled = false;
        Plus.enabled = false;
        Confirm.enabled = false;
        ShowBid.enabled = false;
        Dice.SetActive(false);
        mainController = playerController;
        CurrentBid = currentBid;
        CurrentBidDice = currentBidDice;
        bid = currentBid;
        bidDice = currentBidDice;
        StartRotation(mainController, bidDice, bid);
        if(first)
        {
            Bluff.enabled = false;
        }
        else  
        {
            Bluff.enabled = true;
        }

    }
}
