using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject parentObj;
    [SerializeField] public GameObject[] tetromino;
    public GameObject obj;
    public Vector2 spawnPoint = new Vector2(4, 18);
    private int _random;
    private bool start;
    private List<GameObject> _blocks = new List<GameObject>();
    
    private void Start()
    {
        InstantiateObj();
    }

    private void Update()
    {
        
        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i].transform.childCount < 1)
            {
                Destroy(_blocks[i]);
                _blocks.Remove(_blocks[i]);
            }
        }
    }

    public void InstantiateObj()
    {
        if (start)
        {
            _random = gameObject.GetComponent<Duplicate>().randomNumber;
            obj = Instantiate(tetromino[_random], spawnPoint, Quaternion.identity);
        }
        else
        {
            int randomStart = Random.Range(0, tetromino.Length);
            obj = Instantiate(tetromino[randomStart], spawnPoint, Quaternion.identity);
            start = true;
        }
        _blocks.Add(obj);
        obj.transform.parent = parentObj.transform;
        obj.GetComponent<TetrominoMove>().controllerGameObj = gameObject;
        gameObject.GetComponent<Score>().rowMinus = 0;
        if (_random == 0|| _random == 4 || _random == 6)
        {
            if (_random == 0)
            {
                obj.GetComponent<TetrominoMove>().tetrominoI = true;
            }
            obj.GetComponent<TetrominoMove>().twoRotate = true;
        }
        else
        {
            obj.GetComponent<TetrominoMove>().twoRotate = false;
        }
    }
}