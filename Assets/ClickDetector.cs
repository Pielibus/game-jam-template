using Unity.VisualScripting;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] BidChoice bidChoice;
    void OnMouseDown()
    {
        bidChoice.ListenClick(transform.name);
    }
}
