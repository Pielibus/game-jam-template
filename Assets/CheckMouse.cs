using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using PurrNet;


public class CheckMouse : NetworkBehaviour 
{

private Vector3 screenPoint;
    private Vector3 offset;
    private Transform movementSpace;
public bool On = false;
[SerializeField] private InputActionReference lookInput;
[SerializeField] private RollGobelet rollGobelet;

[SerializeField] public float clampXmax;
[SerializeField] public float clampYmax;
[SerializeField] public float clampXmin;
[SerializeField] public float clampYmin;
[SerializeField] public float testY;
[SerializeField] public float testX;

void OnMouseDown()
{
    if(!On)
        return;
    Vector2 look = lookInput.action.ReadValue<Vector2>();
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

    movementSpace = transform.parent;
    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(look.x, look.y, screenPoint.z));
    Vector3 mouseLocalPosition = movementSpace.InverseTransformPoint(mouseWorldPosition);
    offset = transform.localPosition - mouseLocalPosition;
    rollGobelet.movementSpeed = 0f;

}

void OnMouseDrag()
{
    if(!On)
        return;
    Vector2 look = lookInput.action.ReadValue<Vector2>();
    Vector3 curScreenPoint = new Vector3(look.x, look.y, screenPoint.z);

    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
    Vector3 mouseLocalPosition = movementSpace.InverseTransformPoint(mouseWorldPosition);
    Vector3 curPosition = mouseLocalPosition + offset;

    Vector3 previousPosition = transform.position;
    transform.localPosition = new Vector3(
        Mathf.Clamp(curPosition.x, clampXmin, clampXmax),
        Mathf.Clamp(curPosition.y, clampYmin, clampYmax),
        transform.localPosition.z);

    float deltaTime = Time.deltaTime;
    rollGobelet.movementSpeed = deltaTime > 0f ? Vector3.Distance(previousPosition, transform.position) / deltaTime: 0f;

    }

    void OnMouseUp()
    {
        rollGobelet.movementSpeed = 0f;
    }

}