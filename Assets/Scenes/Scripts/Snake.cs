using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rigidbody2D;
    private Vector2 _diraction = Vector2.right;
    private List<Transform> _segment = new List<Transform>();
    public Transform segmentPrefeb;
    public Transform segmentSecondPrefeb;
    public int initialSize = 1;
    private void Start()
    {
        // _diraction = _diraction / 2;
        ResetState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _diraction != Vector2.down && !CheckTurn(Vector2.up))
        {
            _diraction = Vector2.up * moveSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _diraction != Vector2.up && !CheckTurn(Vector2.down))
        {
            _diraction = Vector2.down * moveSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _diraction != Vector2.right && !CheckTurn(Vector2.left))
        {
            _diraction = Vector2.left * moveSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _diraction != Vector2.left && !CheckTurn(Vector2.right))
        {
            _diraction = Vector2.right * moveSpeed;
        }
    }

    private bool CheckTurn(Vector2 direction)
    {
        // Calculate the next head position after turning
        Vector3 nextHeadPosition = transform.position + new Vector3(direction.x, direction.y, 0);

        // Check if any segment is present at the next head position
        foreach (var segment in _segment)
        {
            if (segment.position == nextHeadPosition)
            {
                return true; // There's a segment at the next position, so turning is not allowed
            }
        }

        return false; // No segment found at the next position, turning is allowed
    }

    private void FixedUpdate()
    {
        Debug.Log("head : " + this.transform.position.ToString());
        for (int i = _segment.Count - 1; i > 0; i--)
        {
            _segment[i].position = _segment[i - 1].position; //+ new Vector3(-_diraction.x , -_diraction.y , 0.0f);

            Debug.Log("tail : " + _segment[i].position.ToString());
        }
        var nextPosition = new Vector3(
            this.transform.position.x + _diraction.x,
            this.transform.position.y + _diraction.y,
            0.0f
        );
        this.transform.position = nextPosition;
    }

    private void Grow()
    {
        if (_segment.Count < 4)
        {
            Transform segment = Instantiate(this.segmentSecondPrefeb);
            if (moveSpeed > 5)
            {
                moveSpeed += 0.05f;
            }
            _segment.Add(segment);
        }
        else
        {
            Transform segment = Instantiate(this.segmentPrefeb);
            if (moveSpeed > 5)
            {
                moveSpeed += 0.05f;
            }
            _segment.Add(segment);
        }

        // segment.position = _segment[_segment.Count - 1].position + new Vector3(-_diraction.x * 1.5f, -_diraction.y * 1.5f, 0.0f);

    }
    private void ResetState()
    {
        for (int i = 1; i < _segment.Count; i++)
        {
            Destroy(_segment[i].gameObject);
        }
        moveSpeed = 0.5f;
        _diraction = _diraction * moveSpeed;
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
        if (other.tag == "Food")
        {
            Grow();
        }
        else if (other.tag == "Obstacle")
        {
            ResetState();
        }
        else if (other.tag == "Child")
        {
            ResetState();
        }
    }
}
