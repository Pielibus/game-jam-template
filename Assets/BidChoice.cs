using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BidChoice : MonoBehaviour
{
    private int bid = 1;
    private int bidDice = 1;
    [SerializeField] private Canvas Main;
    [SerializeField] private Button plusBid;
    [SerializeField] private Button minusBid;
    [SerializeField] private Button plusBidDice;
    [SerializeField] private Button minusBidDice;
    [SerializeField] private TextMeshProUGUI textBid;
    [SerializeField] private TextMeshProUGUI textBidDice;
    [SerializeField] private Button Confirm;
    [SerializeField] private Button Bluff;
    [SerializeField] private RoundRunningState roundRunningState;
    public int CurrentBid, CurrentBidDIce;
    private PlayerController mainController;
    void Start()
    {
        plusBid.onClick.AddListener(delegate {ChangeBid(true); });
        minusBid.onClick.AddListener(delegate {ChangeBid(false); });
        minusBidDice.onClick.AddListener(delegate {ChangeBidDice(false); });
        plusBidDice.onClick.AddListener(delegate {ChangeBidDice(true); });
        Confirm.onClick.AddListener(ConfirmChoice);
        Bluff.onClick.AddListener(BluffChoice);
    }

    private void BluffChoice()
    {
        Main.enabled = false;
        roundRunningState.EndGame(mainController);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartRound(PlayerController playerController, int currentBid, int currentBidDice, bool first)
    {
        Main.enabled = true;
        mainController = playerController;
        Cursor.lockState = CursorLockMode.None;
        CurrentBid = currentBid;
        CurrentBidDIce = currentBidDice;
        bid = currentBid;
        bidDice = currentBidDice;
        textBidDice.text = bidDice.ToString();
        textBid.text = bid.ToString();
        if(first)
        {
            Bluff.gameObject.SetActive(false);
        }
        else  
        {
            Bluff.gameObject.SetActive(true);
        }

    }

    void ChangeBid(bool plus)
    {
        if(plus)
        {
            bid += 1;
        }
        else
        {
            if(bid > CurrentBid)
                bid -= 1;
        }
        textBid.text = bid.ToString();
    }
    void ChangeBidDice(bool plus)
    {
        if(plus)
        {
            if(bidDice < 6)
                bidDice += 1;
        }
        else
        {
            if(bidDice > 1)
            bidDice -= 1;
        }
        textBidDice.text = bidDice.ToString();
    }
    void ConfirmChoice()
    {
        Main.enabled = false;
        roundRunningState.EndRound(mainController, bid, bidDice);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
