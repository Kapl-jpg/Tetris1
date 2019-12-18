using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject parentObj;
    [SerializeField] private GameObject[] tetromino;
    private GameObject m_obj;
    public Vector2 spawnPoint = new Vector2(4, 18);
    private int m_randomStart;
    private List<GameObject> _blocks = new List<GameObject>();

    private void Start()
    {
        m_randomStart = Random.Range(0, tetromino.Length);
        InstantiateObj(m_randomStart);
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

    public void InstantiateObj(int random)
    {
        m_obj = Instantiate(tetromino[random], spawnPoint, Quaternion.identity);
        _blocks.Add(m_obj);
        m_obj.transform.parent = parentObj.transform;
        m_obj.GetComponent<TetrominoMove>().controllerGameObj = gameObject;
        gameObject.GetComponent<Score>().rowMinus = 0;
        if (random == 0 || random == 4 || random == 6)
        {
            m_obj.GetComponent<TetrominoMove>().twoRotate = true;
            if (random == 0)
            {
                m_obj.GetComponent<TetrominoMove>().tetrominoI = true;
            }
        }
        else
        {
            m_obj.GetComponent<TetrominoMove>().twoRotate = false;
        }
    }
}