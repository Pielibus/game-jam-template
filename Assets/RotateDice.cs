using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class RotateDice : MonoBehaviour
{
    [SerializeField] public Dictionary<int, Vector3> faces = new();
    [SerializeField] private GameObject physicDiceBid;
    [SerializeField] private TextMeshPro physicBid;
    public Vector3 targetAngle;

    private Vector3 currentAngle;
    private PlayerController player;

    public void StartRotation(PlayerController plr, int bidDice, int bid)
    {
        player = plr;
        currentAngle = transform.eulerAngles;
        targetAngle = faces[bidDice];
        physicBid.text = bid.ToString();
        targetAngle.y += player.OGRotation.y;
        physicBid.transform.parent.eulerAngles = new Vector3(physicBid.transform.parent.eulerAngles.x, player.OGRotation.y, physicBid.transform.parent.eulerAngles.z);
    }

    public void Update()
    {
        if(!player)
            return;

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
            //Mathf.LerpAngle(player.OGRotation.y + currentAngle.y, player.OGRotation.y + targetAngle.y, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

        transform.eulerAngles = currentAngle;
    }
}
