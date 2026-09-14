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

void OnMouseDown()
{
    if(!On)
        return;
    Vector2 look = lookInput.action.ReadValue<Vector2>();
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(look.x, look.y, screenPoint.z));

}

void OnMouseDrag()
{
    if(!On)
        return;
    Vector2 look = lookInput.action.ReadValue<Vector2>();
    Vector3 curScreenPoint = new Vector3(look.x, look.y, screenPoint.z);

    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    Move(curScreenPoint);

}

[ServerRpc]
void Move(Vector3 curScreenPoint)
    {
        transform.position = new Vector3(Math.Clamp((transform.position.x - (curScreenPoint.x)/10), -10, 10), transform.position.y, transform.position.z);
    }

}