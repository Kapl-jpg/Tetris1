using UnityEngine;
using Random = UnityEngine.Random;

public class Duplicate : MonoBehaviour
{
    [SerializeField] private Vector2 spawn;
    [HideInInspector] public int randomNumber;
    [SerializeField] private GameObject[] tetromino;
    private GameObject m_obj;

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
        randomNumber = Random.Range(0, tetromino.Length);
    }

    private void InstantiateObject()
    {
        m_obj = Instantiate(tetromino[randomNumber], spawn, Quaternion.identity);
        m_obj.GetComponent<TetrominoMove>().enabled = false;
    }
}