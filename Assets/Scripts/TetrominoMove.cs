using System;
using UnityEngine;

public class TetrominoMove : MonoBehaviour
{
    public GameObject controllerGameObj;

    [SerializeField] private Vector3 rotatePoint;

    [SerializeField] private float speed;
    [SerializeField] private float stickingTime;

    private static Transform[,] grid = new Transform[W, H];

    private const int W = 10;
    private const int H = 20;

    private float _time;
    private float _timer;
    private float _addSpeed;

    private int _roundToIntX;
    private int _roundToIntY;

    private const int Angle = 90;
    private int rotationRatio;

    private bool rotateI;
    public bool tetrominoI;
    public bool twoRotate;
    private bool _rotate = true;

    Score _scoreScript;

    private void Start()
    {
        _scoreScript = controllerGameObj.GetComponent<Score>();
    }

    private void Update()
    {

        speed = _scoreScript.speed;
        for (int i = 0; i < H; i++)
        {
            if (FullLine(i))
            {
                DeleteRow(i);
                RowDown(i);
            }
        }

        VerticalMove();
        Controller();
    }

    private bool FullLine(int y)
    {
        for (int x = 0; x < W; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    private bool LoseGame()
    {
        if (transform.position.y > controllerGameObj.GetComponent<Spawner>().spawnPoint.y - 4 && !Border())
        {
            controllerGameObj.GetComponent<Score>().lose = true;
            return true;
        }

        return false;
    }

    private void RowDown(int i)
    {
        for (int y = i; y < H; y++)
        {
            for (int x = 0; x < W; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }

    private void DeleteRow(int y)
    {
        _scoreScript.rows++;
        _scoreScript.rowMinus++;
        for (int x = 0; x < W; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

        _scoreScript.add = true;
    }

    private void VerticalMove()
    {
        _time += Time.deltaTime;
        if (_time * _addSpeed > 0.8f / speed)
        {
            transform.position += Vector3.down;
            _time = 0;
        }

        if (!Border())
        {
            transform.position += Vector3.up;
            foreach (Transform child in transform)
            {
                _roundToIntX = Mathf.RoundToInt(child.position.x);
                _roundToIntY = Mathf.RoundToInt(child.position.y);
                grid[_roundToIntX, _roundToIntY] = child;
            }

            this.enabled = false;
            if (!LoseGame())
            {
                controllerGameObj.GetComponent<Spawner>().InstantiateObj();
                controllerGameObj.GetComponent<Duplicate>().NextObj();
            }
        }
    }

    private void Left()
    {
        transform.position += Vector3.left;
        if (!Border())
            transform.position += Vector3.right;
    }

    private void Right()
    {
        transform.position += Vector3.right;
        if (!Border())
            transform.position += Vector3.left;
    }

    private void Down()
    {
        transform.position += Vector3.down;
        if (!Border())
            transform.position += Vector3.up;
    }

    private void Controller()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown((KeyCode.RightArrow)))
        {
            Right();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown((KeyCode.DownArrow)))
        {
            Down();
        }

        if (time_sticking())
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Left();
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey((KeyCode.RightArrow)))
            {
                Right();
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _addSpeed = 12;
            }
            else
            {
                _addSpeed = 1;
            }

            _timer = 0;
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow))
        {
            _timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (twoRotate)
            {
                if (tetrominoI && transform.position.x >= W / 2)
                    rotateI = true;
                else
                    rotateI = false;
                if (!rotateI)
                {
                    if (_rotate)
                    {
                        transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, Angle);
                        _rotate = false;
                    }
                    else
                    {
                        transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, -Angle);
                        _rotate = true;
                    }
                }
                else
                {
                    if (_rotate)
                    {
                        transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, -Angle);
                        _rotate = false;
                    }
                    else
                    {
                        transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, Angle);
                        _rotate = true;
                    }
                }
            }
            else
            {
                transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, Angle);
            }

            if (!Border())
            {
                if (_roundToIntX < 0)
                {
                    transform.position += Vector3.right;
                }
                else if (_roundToIntX >= W)
                {
                    transform.position += Vector3.left;
                }
                else if (_roundToIntY >= H)
                {
                    transform.position += Vector3.down;
                }
            }
        }
    }

    private bool time_sticking()
    {
        if (Input.anyKey)
        {
            _timer += Time.deltaTime;
            if (_timer < stickingTime)
            {
                return false;
            }
        }

        return true;
    }

    private bool Border()
    {
        foreach (Transform child in transform)
        {
            _roundToIntX = Mathf.RoundToInt(child.position.x);
            _roundToIntY = Mathf.RoundToInt(child.position.y);
            if (_roundToIntX < 0 || _roundToIntX >= W ||
                _roundToIntY < 0 || _roundToIntY >= H)
            {
                return false;
            }

            if (grid[_roundToIntX, _roundToIntY] != null)
            {
                return false;
            }
        }

        return true;
    }
}