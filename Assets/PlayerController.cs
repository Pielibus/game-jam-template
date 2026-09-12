using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using PurrNet;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject[] AllDice;
    [SerializeField] private GameObject Gobelet;private NumberChecker numberChecker;
    void Awake()
    {
        numberChecker = GameObject.FindGameObjectWithTag("Ground").GetComponent<NumberChecker>();
    }
    public void launchRolling()
    {
        Debug.Log("Launching dices");
        numberChecker.dicesCount.Clear();
        foreach (var dice in AllDice)
        {
            dice.GetComponent<DiceRoll>().Roll();
        }
        
    }

    public void CheckGobelet(InputAction.CallbackContext context)
    {
            Gobelet.GetComponent<CheckGobelet>().Check(context.performed); 
          
    }
    
}
