using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using PurrNet;


public class CheckMouse : NetworkBehaviour 
{

private Vector3 screenPoint;
private Vector3 offset;
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

    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(look.x, look.y, screenPoint.z));
    rollGobelet.movementSpeed = 0f;

}

void OnMouseDrag()
{
    if(!On)
        return;
    Vector2 look = lookInput.action.ReadValue<Vector2>();
    Vector3 curScreenPoint = new Vector3(look.x, look.y, screenPoint.z);

    Vector3 curPosition =
    Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

    Vector3 previousPosition = transform.position;
    transform.position = new Vector3(
    Mathf.Clamp(curPosition.x, clampXmin, clampXmax),
    Mathf.Clamp(curPosition.y, clampYmin, clampYmax),
    transform.position.z);

    float deltaTime = Time.deltaTime;
    rollGobelet.movementSpeed = deltaTime > 0f ? Vector3.Distance(previousPosition, transform.position) / deltaTime: 0f;

    }

    void OnMouseUp()
    {
        rollGobelet.movementSpeed = 0f;
    }

}