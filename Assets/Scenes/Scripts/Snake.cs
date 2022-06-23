using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private Vector2 _input;
    private Vector2? _oldDirection = null;
    private List<Transform> _segments;
    private bool _paused = false;
    private float _score = 0;
    public Transform segmentPrefab;
    public TMP_Text scoreText;

    private void Start()
    {
        _segments = new();
        _segments.Add(this.transform);
    }
    private void Update()
    {
        // Only allow turning up or down while moving in the x-axis
        if (_direction.x != 0f && !_paused)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _input = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _input = Vector2.down;
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (_direction.y != 0f && !_paused)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _input = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _input = Vector2.left;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!_paused)
            {
                _paused = true;
                scoreText.text = "Paused. Press ESC to resume.";
                _oldDirection = _direction;
                _direction = Vector2.zero;

            }
            else
            {
                _paused = false;
                _direction = _oldDirection.Value;
                scoreText.text = $"Score: {_score:N0}";

            }

        }
    }

    private void FixedUpdate()
    {
        if (_input != Vector2.zero && !_paused)
        {
            _direction = _input;
        }

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        if (!_paused)
        {
            this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f);
        };

    }

    private void Grow()
    {

        // Transform newSegment = Instantiate(this._score % 5 == 0 ? every5thSegmentPrefab : segmentPrefab);
        Transform newSegment = Instantiate(this.segmentPrefab);

        newSegment.position = _segments[_segments.Count - 1].position;

        _segments.Add(newSegment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_paused) return;
        if (other.tag == "Food")
        {
            _score++;
            scoreText.text = $"Score: {_score:N0}";
            Grow();
        }

        if (other.tag == "Obstacle")
        {
            ResetState();
        }
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);
        this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

        _score = 0;
        scoreText.text = $"Score: {0:N0}";
    }
}

