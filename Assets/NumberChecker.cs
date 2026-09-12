using System.Collections.Generic;
using UnityEngine;

public class NumberChecker : MonoBehaviour
{
    public List<GameObject> dicesCount = new List<GameObject>();
    void OnTriggerStay(Collider other)
    {

        if(other.gameObject.GetComponentInParent<Rigidbody>().linearVelocity == Vector3.zero)
            {
                if(!dicesCount.Contains(other.gameObject))
                {
                Debug.Log(other.transform.name);
                dicesCount.Add(other.gameObject);
                }
                
            }     
    }
}
