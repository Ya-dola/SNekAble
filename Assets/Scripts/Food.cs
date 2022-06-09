using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private Bounds _bounds;

    private float _xPos;
    private float _yPos;

    // Start is called before the first frame update
    void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        _bounds = this.gridArea.bounds;

        _xPos = Mathf.Round(Random.Range(_bounds.min.x, _bounds.max.x));
        _yPos = Mathf.Round(Random.Range(_bounds.min.y, _bounds.max.y));

        transform.position = new Vector3(_xPos, _yPos, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Snek"))
        {
            
        }
        RandomizePosition();
    }
}