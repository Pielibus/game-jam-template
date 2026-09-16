using System;
using PurrNet;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class DiceRoll : NetworkBehaviour
{
    [SerializeField] private float maxRandomForce = 100, startRollingForce = 200;
    [SerializeField] public Transform center1;
    [SerializeField] public Transform center2;
    [SerializeField] public Transform center3;

    void Start()
    {
        if(!isServer)
        {
            
        }
    }

    public void Roll()
    {       GetComponent<Rigidbody>().isKinematic = false;
            float forceX = Random.Range(0, maxRandomForce);
            float forceY = Random.Range(0, maxRandomForce);
            float forceZ = Random.Range(0, maxRandomForce);
            GetComponent<Rigidbody>().AddForce(Vector3.up * startRollingForce);
            GetComponent<Rigidbody>().AddTorque(forceX, forceY, forceZ);  
        
    }
    void LateUpdate()
    {
        if(!isServer)
            return;
        if((transform.position - center1.position).magnitude > 0.3 && (transform.position - center2.position).magnitude > 0.4 && (transform.position - center3.position).magnitude > 0.4)
        {
            Transform closest = center1;
            if((transform.position - center2.position).magnitude < (transform.position - closest.position).magnitude)
                closest = center2;
            if((transform.position - center3.position).magnitude < (transform.position - closest.position).magnitude)
                closest = center3;
            transform.position = closest.position;
        }
    }
}
