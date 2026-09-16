using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class RotateDice : MonoBehaviour
{
    [SerializeField] public Dictionary<int, Vector3> faces = new();
    [SerializeField] private GameObject physicDiceBid;
    [SerializeField] private TextMeshPro physicBid;
    public Vector3 targetAngle;
    private bool start = false;
    private Vector3 currentAngle;
    public PlayerController player;
    private Quaternion targetRotation;

    public void StartRotation(int bidDice, int bid)
    {
        if(bidDice == 0)
        {
            physicBid.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            return;
        }
        physicBid.enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        Quaternion playerRotation = Quaternion.Euler(0f, player.OGRotation.y, 0f);
        targetRotation = playerRotation * Quaternion.Euler(faces[bidDice]);
        currentAngle = transform.eulerAngles;
        targetAngle = targetRotation.eulerAngles;
        physicBid.text = bid.ToString();
        physicBid.transform.parent.rotation = playerRotation;
        start = true;
    }

    public void Update()
    {
        if(!start)
            return;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}
