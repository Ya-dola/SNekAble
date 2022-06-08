using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snek : MonoBehaviour
{
    public Vector2 moveDir;

    public float speed;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(
            Mathf.Round(transform.position.x) + moveDir.x,
            Mathf.Round(transform.position.y) + moveDir.y,
            transform.position.z), Time.fixedDeltaTime * speed);
    }
}