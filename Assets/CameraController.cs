using PurrNet;
using PurrNet.Lobby;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private InputActionReference lookInput;
    [SerializeField] private GameObject Head;
    [SerializeField] private float mouseSensitivity;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public float maxClampy = 90f;
    public float maxClampx = 270f;
    private Vector3 initialRotation;
    private bool _inputBlocked;
    private PlayerInput _playerInput;
    private InputAction _lookAction;

    private void Awake()
    {
        _playerInput = transform.parent.GetComponentInParent<PlayerInput>();
        _lookAction = _playerInput.actions.FindAction("Look");
    }

    private void OnEnable()
    {
        PauseMenuView.onOpened += BlockInput;
        PauseMenuView.onClosed += AllowInput;
    }

    private void OnDisable()
    {
        PauseMenuView.onOpened -= BlockInput;
        PauseMenuView.onClosed -= AllowInput;
    }
    private void BlockInput()
    {
        _inputBlocked = true;
    }

    private void AllowInput()
    {
        _inputBlocked = false;
    }

    
    void Start()
    {
        if(!isOwner)
            return;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        initialRotation = Head.transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        if(_inputBlocked || !isOwner || _playerInput == null || !_playerInput.enabled)
            return;
        Vector2 look = _lookAction.ReadValue<Vector2>();
        if (Mouse.current != null && Mouse.current.delta.IsActuated())
        {
            look *= mouseSensitivity;
        }
        xRotation -= look.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation -= look.x;
        yRotation = Mathf.Clamp(yRotation, initialRotation.y + (-90f), initialRotation.y + 90f);
        Head.transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
    }
}
