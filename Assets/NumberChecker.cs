using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberChecker : MonoBehaviour
{

    [SerializeField] public Dictionary<GameObject, int> dicesCount = new();
    void OnTriggerStay(Collider other)
    {

        if(other.gameObject.GetComponentInParent<Rigidbody>().linearVelocity == Vector3.zero)
            {
                if(!dicesCount.ContainsKey(other.gameObject))
                {
                    dicesCount.Add(other.gameObject, int.Parse(other.transform.name));
                }
                
            }     
    }
}
