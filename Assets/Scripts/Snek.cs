using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snek : MonoBehaviour
{
    [Header("Movement Vectors")]
    public Vector2 newMoveDir;

    public Vector2 currentMoveDir;

    [Header("Movement Smoothness")]
    public float smoothStep = 0.1f;

    // Read Value of Movement from New Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        newMoveDir = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Movement Coroutine
        StartCoroutine(MoveSnekConfigRout());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        ChangeDir();
    }

    // Change Direction of Movement and Keep it Persistent
    private void ChangeDir()
    {
        // Up Direction
        if (newMoveDir.Equals(new Vector2(0f, 1f)))
        {
            currentMoveDir = new Vector2(0f, 1f);
        }

        // Down Direction
        if (newMoveDir.Equals(new Vector2(0f, -1f)))
        {
            currentMoveDir = new Vector2(0f, -1f);
        }

        // Left Direction
        if (newMoveDir.Equals(new Vector2(-1f, 0f)))
        {
            currentMoveDir = new Vector2(-1f, 0f);
        }

        // Right Direction
        if (newMoveDir.Equals(new Vector2(1f, 0f)))
        {
            currentMoveDir = new Vector2(1f, 0f);
        }
    }

    IEnumerator MoveSnekConfigRout()
    {
        while (true)
        {
            // Calls for Lerping Movement after Movement finishes
            yield return StartCoroutine(MoveSnekRout(
                new Vector3(Mathf.Round(transform.position.x) + currentMoveDir.x,
                    Mathf.Round(transform.position.y) + currentMoveDir.y,
                    transform.position.z), smoothStep));
        }
    }

    IEnumerator MoveSnekRout(Vector3 newPos, float duration)
    {
        // Timer for Lerp
        float time = 0;

        // Total time allocated for Lerp
        while (time < duration)
        {
            // Division of Time Elapsed by total time allocated for lerp
            transform.position = Vector3.Lerp(transform.position, newPos, time / duration);

            // Incrementing timer on Every Frame for Lerp
            time += Time.fixedDeltaTime;

            yield return null;
        }

        // Ensure New Position is the Final Position after Lerp
        transform.position = newPos;
    }
}