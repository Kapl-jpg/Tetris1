using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Duplicate : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawn;
    public int randomNumber;
    [SerializeField]
    private GameObject[] tetromino;
    private GameObject _obj;

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
        Destroy(_obj);
    }
    private void RandomTetromino()
    {
        randomNumber = Random.Range(0, tetromino.Length);
    }

    private void InstantiateObject()
    {
        _obj = Instantiate(tetromino[randomNumber], spawn, Quaternion.identity);
        _obj.GetComponent<TetrominoMove>().enabled = false;
    }
}