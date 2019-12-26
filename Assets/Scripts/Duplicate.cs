using UnityEngine;
using Random = UnityEngine.Random;

public class Duplicate : MonoBehaviour
{
    [SerializeField] private Vector2 m_spawn;
    private int m_randomNumber;
    [SerializeField] private GameObject[] m_tetromino;
    private GameObject m_obj;
    private bool m_lose;

    public bool Lose
    {
        get => m_lose;
        set => m_lose = value;
    }

    public int RandomNumber
    {
        get => m_randomNumber;
    }

    public void NextObj()
    {
        Destruction();
        RandomTetromino();
        InstantiateObject();
    }

    private void Start()
    {
        RandomTetromino();
        InstantiateObject();
    }

    private void Destruction()
    {
        Destroy(m_obj);
    }

    private void RandomTetromino()
    {
        m_randomNumber = Random.Range(0, m_tetromino.Length);
    }

    private void InstantiateObject()
    {
        m_obj = Instantiate(m_tetromino[m_randomNumber], m_spawn, Quaternion.identity);
        m_obj.GetComponent<TetrominoMove>().enabled = false;
    }
}