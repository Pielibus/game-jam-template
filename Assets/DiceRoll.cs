using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class DiceRoll : MonoBehaviour
{
    [SerializeField] private float maxRandomForce = 100, startRollingForce = 200;

    public void Roll()
    {
            float forceX = Random.Range(0, maxRandomForce);
            float forceY = Random.Range(0, maxRandomForce);
            float forceZ = Random.Range(0, maxRandomForce);
            GetComponent<Rigidbody>().AddForce(Vector3.up * startRollingForce);
            GetComponent<Rigidbody>().AddTorque(forceX, forceY, forceZ);  
        
    }
}
