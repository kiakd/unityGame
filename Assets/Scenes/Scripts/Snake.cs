using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _diraction = Vector2.right;
    private List<Transform> _segment = new List<Transform>();
    public Transform segmentPrefeb;
    public int initialSize = 1;
    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            _diraction = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            _diraction = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            _diraction = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            _diraction = Vector2.right;
        } 
    }

    private void FixedUpdate() 
    {
        for (int i = _segment.Count - 1; i > 0; i--)
        {
            _segment[i].position = _segment[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _diraction.x,
            Mathf.Round(this.transform.position.y) + _diraction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefeb);
        segment.position = _segment[_segment.Count - 1].position;
        _segment.Add(segment);
    }
    private void ResetState()
    {
        for (int i = 1; i < _segment.Count; i++)
        {
            Destroy(_segment[i].gameObject);
        }

        _segment.Clear();
        _segment.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++)
        {
            _segment.Add(Instantiate(this.segmentPrefeb));
        }

        this.transform.position = Vector3.zero;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Food")
        {
            Grow();    
        } else if (other.tag == "Obstacle")
        {
            ResetState();
        } else if (other.tag == "Child")
        {
            ResetState();
        }
    }
}
