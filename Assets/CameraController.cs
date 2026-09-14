using PurrNet;
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
        Vector2 look = lookInput.action.ReadValue<Vector2>();
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
