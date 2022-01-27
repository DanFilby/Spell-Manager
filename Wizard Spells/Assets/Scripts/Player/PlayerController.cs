using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveDirection;
    private Vector3 moveVec;

    public float mouseSensitivity = 100f;
    private Camera cam;
    private Vector2 lookDirection;
    
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        transform.Translate(moveVec, Space.Self);
    }

    private void Look()
    {
        transform.Rotate(new Vector3(0, lookDirection.x, 0) * mouseSensitivity);
        cam.transform.Rotate(new Vector3(lookDirection.y,0,0) * mouseSensitivity * -1);
    }

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        moveVec = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime;
    }

    public void GetLookInput(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();

    }
}


