using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CheckGobelet : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animation;
    private bool checking = false;
    private bool asPause = false;

    public void Check(bool Up)
    {
        if(Up)
        {
            animator.SetTrigger("Check");
            checking = true;
            asPause = false;
        
        }
        else
        {
            if(!checking)
                return;
            if(asPause)
            {
                animator.speed = 1; 
              animator.SetTrigger("UnCheck");
            }
            else
            {
                animator.speed = 1; 
                animator.SetTrigger("UnCheck");
            }
            
            checking = false;
        }
        
        
    }
    public void Pause()
    {
        if(checking)
        {
           animator.speed = 0; 
        }
        asPause = true;
            
    }
}
