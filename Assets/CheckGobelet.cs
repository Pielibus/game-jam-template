using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CheckGobelet : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool checking = false;

    public void Check(bool Up)
    {
        if(Up)
        {
            animator.SetTrigger("Check");
            checking = true; 
        }
        else
        {
            animator.speed = 1;
            checking = false;
        }
        
    }
    public void Pause()
    {
        if(checking)
        {
           animator.speed = 0; 
        }
            
    }
}
