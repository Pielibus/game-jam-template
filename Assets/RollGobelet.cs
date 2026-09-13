using UnityEngine;
using UnityEngine.InputSystem;
public class RollGobelet : MonoBehaviour
{
    [SerializeField] private GameObject gobelet;
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference lookInput;
    [SerializeField] private CheckMouse checkMouse;
    [SerializeField] private MeshCollider wall;
    [SerializeField] private BoxCollider close;
    private bool RollLock = false;
    public void StartRoll()
    {
        animator.SetTrigger("StartRoll");
        wall.enabled = false;
        close.enabled = true;
    }
    public void PauseRoll()
    {
        animator.speed = 0;
        animator.enabled = false;
        checkMouse.On = true;
    }
}
