using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject m_parentObj;
    [SerializeField] private GameObject[] m_tetromino;
    private GameObject m_obj;
    private Vector2 spawnPoint = new Vector2(4, 18);

    public Vector2 SpawnPoint
    {
        get => spawnPoint;
    }

    private List<GameObject> _blocks = new List<GameObject>();

    private void Start()
    {
        int randomStart = Random.Range(0, m_tetromino.Length);
        InstantiateObj(randomStart);
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
        m_obj = Instantiate(m_tetromino[random], spawnPoint, Quaternion.identity);
        _blocks.Add(m_obj);
        m_obj.transform.parent = m_parentObj.transform;
        m_obj.GetComponent<TetrominoMove>().ControllerGameObj = gameObject;
        gameObject.GetComponent<Score>().RowMinus = 0;
    }
}