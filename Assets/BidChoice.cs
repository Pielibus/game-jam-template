using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BidChoice : MonoBehaviour
{
    private int bid = 1;
    private int bidDice = 1;
    [SerializeField] private Button plusBid;
    [SerializeField] private Button minusBid;
    [SerializeField] private Button plusBidDice;
    [SerializeField] private Button minusBidDice;
    [SerializeField] private TextMeshProUGUI textBid;
    [SerializeField] private TextMeshProUGUI textBidDice;
    void Start()
    {
        plusBid.onClick.AddListener(delegate {ChangeBid(true); });
        minusBid.onClick.AddListener(delegate {ChangeBid(false); });
        minusBidDice.onClick.AddListener(delegate {ChangeBidDice(false); });
        plusBidDice.onClick.AddListener(delegate {ChangeBidDice(true); });
    }

    
    void Update()
    {
        
    }

    void ChangeBid(bool plus)
    {
        if(plus)
        {
            bid += 1;
        }
        else
        {
            bid -= 1;
        }
        textBid.text = bid.ToString();
    }
    void ChangeBidDice(bool plus)
    {
        if(plus)
        {
            bidDice += 1;
        }
        else
        {
            bidDice -= 1;
        }
        textBidDice.text = bidDice.ToString();
    }
}
