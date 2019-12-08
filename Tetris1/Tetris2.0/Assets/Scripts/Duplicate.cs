using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicate : MonoBehaviour
{
    public Vector2 spawn;
    public int rand;
    public GameObject[] Tetromino;
    private GameObject obj;

    public void NextObj()
    {
        Destr();
        random();
        Instant();
    }
    void Start()
    {
        random();
        Instant();
    }
    public void Destr()
    {
        Destroy(obj);
    }
    void random()
    {
        rand = Random.Range(0, Tetromino.Length);
    }
    void Instant()
    {
        obj = Instantiate(Tetromino[rand], spawn, Quaternion.identity);
    }
    void Update()
    {

    }
}