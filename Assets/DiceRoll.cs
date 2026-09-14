using System;
using PurrNet;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class DiceRoll : NetworkBehaviour
{
    [SerializeField] private float maxRandomForce = 100, startRollingForce = 200;
    [SerializeField] private Transform center;

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
        Debug.Log((transform.position - center.position).magnitude + " " + transform.name);
        if((transform.position - center.position).magnitude > 0.4)
        {
            transform.position = center.position;
        }
    }
}
