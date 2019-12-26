using System;
using UnityEngine;


public class TetrominoMove : MonoBehaviour
{
    private GameObject m_controllerGameObj;

    public GameObject ControllerGameObj
    {
        set => m_controllerGameObj = value;
    }

    [SerializeField] private Vector3 m_rotatePoint;

    [SerializeField] private float m_speed = 1;
    [SerializeField] private float m_stickingTime = 0.12f;

    private static Transform[,] grid = new Transform[W,H];
    
    private const int W = 10;
    private const int H = 20;
    private float m_time;
    private float m_timer;
    private float m_addSpeed = 1;
    private int m_inversely;
    private const int ANGLE = 90;
    private int m_turn;
    [SerializeField] private bool m_tetrominoI;
    [SerializeField] private bool m_twoRotate;
    private bool m_rotate = true;
    private bool m_returnRotation;
    private bool m_canRotate;
    private int m_speedX;
    private Score m_scoreScript;
    private Spawner m_spawnerScript;
    private Duplicate m_duplicateScript;
    [SerializeField]private int m_hightTetromino;
    private bool m_loseGame;


    private bool Border()
    {
        foreach (Transform child in transform)
        {
            int roundToIntX = Mathf.RoundToInt(child.position.x);
            int roundToIntY = Mathf.RoundToInt(child.position.y);
            if (roundToIntX < 0 || roundToIntX >= W ||
                roundToIntY < 0 || roundToIntY >= H)
            {
                return false;
            }

            if (grid[roundToIntX, roundToIntY] != null &&
                grid[roundToIntX, roundToIntY].parent != transform)
            {
                return false;
            }
        }

        return true;
    }
    private void Start()
    {
        m_duplicateScript = m_controllerGameObj.GetComponent<Duplicate>();
        m_scoreScript = m_controllerGameObj.GetComponent<Score>();
        m_spawnerScript = m_controllerGameObj.GetComponent<Spawner>();
        if (!Border())
        {
            m_loseGame = true;
            m_scoreScript.Lose = true;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        m_speed = m_scoreScript.Speed;
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

  /*  private bool LoseGame()
    {
        foreach (Transform child in transform)
        {
            if (child.position.y > H - m_hightTetromino && Border())
            {
                m_scoreScript.Lose = true;
                return true;
            }
        }

        return false;
    }*/

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
        m_scoreScript.Rows++;
        m_scoreScript.RowMinus++;
        for (int x = 0; x < W; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

        m_scoreScript.add = true;
    }

    private void VerticalMove()
    {
        m_time += Time.deltaTime;
        if (m_time * m_addSpeed > 0.8f / m_speed)
        {
            transform.position += Vector3.down;
            m_time = 0;
        }

        if (!Border())
        {
            transform.position += Vector3.up;
            foreach (Transform child in transform)
            {
                int roundToIntX = Mathf.RoundToInt(child.position.x);
                int roundToIntY = Mathf.RoundToInt(child.position.y);
                grid[roundToIntX, roundToIntY] = child;
            }

            enabled = false;
           if (!m_loseGame)
            {
                m_spawnerScript.InstantiateObj(m_duplicateScript.RandomNumber);
                m_duplicateScript.NextObj();
            }
        }
    }

    void Translate(float speedX)
    {
        transform.position += Vector3.right * speedX;
        if (!Border())
            transform.position += Vector3.left * speedX;
    }



    private void Controller()
    {
        if (time_sticking())
        {

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                m_speedX = -1;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey((KeyCode.RightArrow)))
            {
                m_speedX = 1;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                m_addSpeed = 12;
            }

            m_timer = 0;
            Translate(m_speedX);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_speedX = -1;
                Translate(m_speedX);
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown((KeyCode.RightArrow)))
            {
                m_speedX = 1;
                Translate(m_speedX);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                m_addSpeed = 12;
            }
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow))
        {
            m_speedX = 0;
            m_timer = 0;
            m_addSpeed = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_twoRotate)
            {
                if (m_tetrominoI)
                {
                    if (transform.position.x > W / 2)
                    {
                        if (transform.rotation.z >= 0)
                        {
                            m_rotate = false;
                        }
                    }
                    else
                    {
                        if (transform.rotation.z <= 0)
                        {
                            m_rotate = true;
                        }
                    }
                }

                if (m_rotate)
                {
                    m_turn = ANGLE;
                    m_returnRotation = true;
                    m_rotate = false;
                }
                else
                {
                    m_turn = -ANGLE;
                    m_returnRotation = false;
                    m_rotate = true;
                }
            }
            else
            {
                m_turn = ANGLE;
                m_returnRotation = true;
            }

            Rotate(m_turn, m_returnRotation, m_inversely, false);
            if (!Border())
            {
                foreach (Transform child in transform)
                {
                    int roundToIntX = Mathf.RoundToInt(child.position.x);
                    int roundToIntY = Mathf.RoundToInt(child.position.y);
                    if (roundToIntX < 0)
                    {
                        transform.position += Vector3.right;
                        m_inversely = -1;
                    }
                    else if (roundToIntX >= W)
                    {
                        transform.position += Vector3.left;
                        m_inversely = 1;
                    }
                    else if (roundToIntY >= H)
                    {
                        transform.position += Vector3.down;
                    }
                }
            }

            if (!Border())
            {
                Rotate(m_turn, m_returnRotation, m_inversely, true);
            }
        }
    }

    void Rotate(int angle, bool back, int fallBack, bool backRotate)
    {
        if (backRotate)
        {
            if (back)
            {
                angle = -ANGLE;
            }
            else
            {
                angle = ANGLE;
            }

            transform.position += new Vector3(fallBack, 0);
        }

        transform.RotateAround(transform.TransformPoint(m_rotatePoint), Vector3.forward, angle);
    }

    private bool time_sticking()
    {
        if (Input.anyKey)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_stickingTime)
            {
                return true;
            }
        }

        return false;
    }

    
}