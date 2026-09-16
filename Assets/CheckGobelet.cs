using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using PurrNet;

public class CheckGobelet : NetworkBehaviour
{
    [SerializeField] private NetworkAnimator animator;
    private bool checking = false;
    private bool asPause = false;

    [TargetRpc]
    public void Check(PlayerID playerID, bool Up)
    {
        if(Up)
        {
            animator.animator.SetTrigger("Check");
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
