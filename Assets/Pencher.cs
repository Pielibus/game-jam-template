using UnityEngine;
using UnityEngine.InputSystem;
using PurrNet.StateMachine;
using PurrNet;
public class Pencher : MonoBehaviour
{
    [SerializeField] private NetworkAnimator animator;
    private bool pencherGauche = false;
    private bool pencherDroite = false;
    public void StartPencherDroite(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            if(!pencherDroite)
            {
                Debug.Log("PencherDroite");
                pencherDroite = true;
                animator.SetTrigger("PencherDroite");
            }
                
        }
        else
        {
            StopPencher();
        }
    }
    public void StartPencherGauche(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            if(!pencherGauche)
            {
                Debug.Log("PencherGauche");
                pencherGauche = true;
                animator.SetTrigger("PencherGauche");
            }
        }
        else
        {
            StopPencher();
        }
        
    }
    public void PausePencher()
    {
        if(pencherGauche || pencherDroite)
            animator.speed = 0;
    }
    public void StopPencher()
    {
        animator.SetTrigger("Stop");
        animator.speed = 1;
        pencherDroite = false;
        pencherGauche = false;
    }
}
