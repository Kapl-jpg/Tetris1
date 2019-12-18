using UnityEngine;

public class TetrominoMove : MonoBehaviour
{
    [HideInInspector] public GameObject controllerGameObj;

    [SerializeField] private Vector3 rotatePoint;

    [SerializeField] private float speed;
    [SerializeField] private float stickingTime;

    private static Transform[,] grid = new Transform[W, H];

    private const int W = 10;
    private const int H = 20;

    [SerializeField] private Vector2 borderLose = new Vector2(1, 0);
    private float m_time;
    private float m_timer;
    private float m_addSpeed = 1;
    private int m_roundToIntX;
    private int m_roundToIntY;
    private const int ANGLE = 90;
    private int m_turn;
    [HideInInspector] public bool tetrominoI;
    [HideInInspector] public bool twoRotate;
    private bool m_rotate = true;
    private bool m_returnRotation;
    private int m_speedX;
    private Score m_scoreScript;
    private Spawner m_spawnerScript;
    private Duplicate m_duplicateScript;

    private void Start()
    {
        m_duplicateScript = controllerGameObj.GetComponent<Duplicate>();
        m_scoreScript = controllerGameObj.GetComponent<Score>();
        m_spawnerScript = controllerGameObj.GetComponent<Spawner>();
    }

    private void Update()
    {
        speed = m_scoreScript.speed;
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
        foreach (Transform child in transform)
        {
            if (child.position.y > m_spawnerScript.spawnPoint.y - borderLose.y
                && Border() && child.position.x < m_spawnerScript.spawnPoint.x + borderLose.x &&
                child.position.x > m_spawnerScript.spawnPoint.x - borderLose.x)
            {
                m_scoreScript.lose = true;
                return true;
            }
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
        m_scoreScript.rows++;
        m_scoreScript.rowMinus++;
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
        if (m_time * m_addSpeed > 0.8f / speed)
        {
            transform.position += Vector3.down;
            m_time = 0;
        }

        if (!Border())
        {
            transform.position += Vector3.up;
            foreach (Transform child in transform)
            {
                m_roundToIntX = Mathf.RoundToInt(child.position.x);
                m_roundToIntY = Mathf.RoundToInt(child.position.y);
                grid[m_roundToIntX, m_roundToIntY] = child;
            }

            enabled = false;
            if (!LoseGame())
            {
                m_spawnerScript.InstantiateObj(m_duplicateScript.randomNumber);
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

    void Rotate(int angle)
    {
        transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, angle);
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
            if (twoRotate)
            {
                if (tetrominoI)
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

            Rotate(m_turn);
            if (!Border() && CanRotate())
            {
                if (m_roundToIntX < 0)
                {
                    transform.position += Vector3.right;
                }
                else if (m_roundToIntX >= W)
                {
                    transform.position += Vector3.left;
                }
                else if (m_roundToIntY >= H)
                {
                    transform.position += Vector3.down;
                }
            }

            if (!Border() && !CanRotate())
            {
                BackRotation(m_returnRotation);
            }
        }
    }

    void BackRotation(bool back)
    {
        if (back)
            transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, -ANGLE);
        else
            transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, ANGLE);
    }

    bool CanRotate()
    {
        if (m_roundToIntX < 0 || m_roundToIntX >= W || m_roundToIntY >= H)
        {
            return true;
        }

        return false;
    }

    private bool time_sticking()
    {
        if (Input.anyKey)
        {
            m_timer += Time.deltaTime;
            if (m_timer > stickingTime)
            {
                return true;
            }
        }

        return false;
    }

    private bool Border()
    {
        foreach (Transform child in transform)
        {

            m_roundToIntX = Mathf.RoundToInt(child.position.x);
            m_roundToIntY = Mathf.RoundToInt(child.position.y);
            if (m_roundToIntX < 0 || m_roundToIntX >= W ||
                m_roundToIntY < 0 || m_roundToIntY >= H)
            {
                return false;
            }

            if (grid[m_roundToIntX, m_roundToIntY] != null &&
                grid[m_roundToIntX, m_roundToIntY].parent != transform)
            {
                return false;
            }
        }

        return true;
    }
}