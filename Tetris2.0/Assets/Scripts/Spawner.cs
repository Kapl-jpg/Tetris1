using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject[] Tetromino;
    public GameObject obj;
    public Vector2 Spawn_point = new Vector2(4, 19);
    private bool random;
    public int random1;

    void Start()
    {
        int randomize = Random.Range(0, Tetromino.Length);
        obj = Instantiate(Tetromino[randomize], Spawn_point, Quaternion.identity);
    }

    private void Update()
    {
        random1 = gameObject.GetComponent<Duplicate>().rand;
    }

    public void Instan()
    {
        obj = Instantiate(Tetromino[random1], Spawn_point, Quaternion.identity);
    }
}
