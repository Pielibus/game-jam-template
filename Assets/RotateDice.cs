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
        currentAngle = transform.eulerAngles;
        targetAngle = faces[bidDice];
        physicBid.text = bid.ToString();
        Debug.Log(player.OGRotation.y);
        targetAngle.y += player.OGRotation.y;
        physicBid.transform.parent.eulerAngles = new Vector3(physicBid.transform.parent.eulerAngles.x, player.OGRotation.y, physicBid.transform.parent.eulerAngles.z);
        start = true;
    }

    public void Update()
    {
        if(!start)
            return;
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
            //Mathf.LerpAngle(player.OGRotation.y + currentAngle.y, player.OGRotation.y + targetAngle.y, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

        transform.eulerAngles = currentAngle;
    }
}
