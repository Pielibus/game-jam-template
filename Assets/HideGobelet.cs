using UnityEngine;
using UnityEngine.InputSystem;
using PurrNet.StateMachine;
using PurrNet;
public class HideGobelet : MonoBehaviour
{
    [SerializeField] private NetworkAnimator animator;
    private bool HideGauche = false;
    private bool HideDroite = false;
    public void StartHideDroite(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            if(!HideDroite)
            {
                HideDroite = true;
                animator.SetTrigger("HideDroite");
            }
                
        }
        else
        {
            StopHide();
        }
    }
    public void StartHideGauche(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            if(!HideGauche)
            {
                HideGauche = true;
                animator.SetTrigger("HideGauche");
            }
        }
        else
        {
            StopHide();
        }
        
    }
    public void PauseHide()
    {
        if(HideGauche || HideDroite)
            animator.speed = 0;
    }
    public void StopHide()
    {
        animator.SetTrigger("Stop");
        animator.speed = 1;
        HideDroite = false;
        HideGauche = false;
    }
}
