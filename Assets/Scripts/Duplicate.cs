using UnityEngine;
using Random = UnityEngine.Random;

public class Duplicate : MonoBehaviour
{
    public Vector2 spawn;
    public int randomNumber;
    public GameObject[] tetromino;
    private GameObject _obj;

    public void NextObj()
    {
        Destruction();
        RandomFigure();
        InstantiateObject();
    }

    void Start()
    {
        RandomFigure();
        InstantiateObject();
    }

    void Destruction()
    {
        Destroy(_obj);
    }

    void RandomFigure()
    {
        randomNumber = Random.Range(0, tetromino.Length);
    }

    void InstantiateObject()
    {
        _obj = Instantiate(tetromino[randomNumber], spawn, Quaternion.identity);
        _obj.GetComponent<TetrominoMove>().enabled = false;
    }
}