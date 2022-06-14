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

    [Header("Segments")]
    public Transform segmentPrefab;

    private List<Transform> _segments;

    // Read Value of Movement from New Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        newMoveDir = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initializing the Segments List
        _segments = new List<Transform>();

        // Adding Head as 1st Segment to List
        _segments.Add(transform);

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
            /* Go through entire snake segments from tail to head,
             * moving current segment to it's predecessors position 
             */
            for (int i = _segments.Count - 1; i > 0; i--)
            {
                // Lerping Movement of Segments
                StartCoroutine(MoveSnekSegRout(new Vector3(
                        Mathf.Round(_segments[i - 1].position.x),
                        Mathf.Round(_segments[i - 1].position.y),
                        _segments[i - 1].position.z)
                    , i, smoothStep));
            }

            // Calls for Lerp-ing Movement after Movement finishes
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

    IEnumerator MoveSnekSegRout(Vector3 newPos, int counter, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            _segments[counter].position = Vector3.Lerp(
                _segments[counter].position,
                newPos,
                time / duration);

            time += Time.fixedDeltaTime;

            yield return null;
        }

        _segments[counter].position = newPos;
    }

    // Grow The body of the snake the more food you eat
    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);

        // Setting position of New Segment to the Position of the Last Segment on the Tail 
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Collision with Food
        if (col.CompareTag("Food"))
            // Grow Segments when eating Normal Food
            Grow();

        // Collision with Obstacles
        if (col.CompareTag("Obstacle"))
        {
            print("You Lost");

            // Stop Everything
            Time.timeScale = 0f;
            StopAllCoroutines();

            // Call Lose Screen
        }
    }
}