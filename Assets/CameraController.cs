using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputActionReference lookInput;
    [SerializeField] private GameObject Head;
    [SerializeField] private float mouseSensitivity;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public float maxClampy = -90f;
    public float maxClampx = 90f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        xRotation = Mathf.Clamp(xRotation, maxClampy, maxClampx);
        yRotation -= look.x;
        yRotation = Mathf.Clamp(yRotation, 90f, 270f);
        Head.transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
    }
}
