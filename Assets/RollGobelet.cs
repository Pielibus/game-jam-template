using UnityEngine;
using UnityEngine.InputSystem;
using PurrNet.StateMachine;
using PurrNet;
public class RollGobelet : MonoBehaviour
{
    [SerializeField] private GameObject gobelet;
    [SerializeField] private NetworkAnimator animator;
    [SerializeField] private InputActionReference lookInput;
    [SerializeField] private CheckMouse checkMouse;
    [SerializeField] private MeshCollider wall;
    [SerializeField] private BoxCollider close;

    private Vector3 OGpos;
    private bool RollLock = false;
    public void StartRoll()
    {
        Debug.Log("StartingRoll");
        animator.SetTrigger("StartRoll");
        wall.enabled = false;
        close.enabled = true;
    }
    public void PauseRoll()
    {
        animator.speed = 0;
        OGpos = checkMouse.transform.position;
        animator.enabled = false;
        checkMouse.On = true;
    }
    public void StopRoll()
    {
        checkMouse.On = false;
        animator.enabled = true;
        animator.speed = 1;
        checkMouse.transform.position = OGpos;
    }
}
