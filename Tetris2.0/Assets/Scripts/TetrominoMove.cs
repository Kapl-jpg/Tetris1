using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TetrominoMove : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    private float time;
    public GameObject Cntrlr;
    public Vector3 Rotate_point;
    static Transform[,] grid = new Transform[w, h];
    private float timer;
    public float speed;
    private float addSpeed;
    public void Start()
    {
        Cntrlr = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        speed = Cntrlr.GetComponent<Score>().speed;
        for (int i = 0; i < h; i++)
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
    bool FullLine(int y)
    {
        for (int x = 0; x < w; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }


    bool LoseGame()
    {
        if (transform.position.y > 15 && !border())
        {
            Cntrlr.GetComponent<Score>().Lose = true;
            return true;
        }
        return false;
    }

    void RowDown(int i)
    {
        for (int y = i; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].transform.position += new Vector3(0, -1, 0);
                }
            }
        }
    }
    void DeleteRow(int y)
    {
        Cntrlr.GetComponent<Score>().rows++;
        Cntrlr.GetComponent<Score>().row++;
        Cntrlr.GetComponent<Score>().add = true;
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }
    void VerticalMove()
    {
        time += Time.deltaTime;
        if (time*addSpeed > 0.8f / speed)
        {
            transform.position += new Vector3(0, -1, 0);
            time = 0;
        }
        if (!border())
        {
            transform.position += new Vector3(0, 1, 0);
            foreach (Transform child in transform)
            {
                int RoundToIntX = Mathf.RoundToInt(child.transform.position.x);
                int RoundToIntY = Mathf.RoundToInt(child.transform.position.y);
                grid[RoundToIntX, RoundToIntY] = child;
            }
            this.enabled = false;
            if (!LoseGame())
            {
                Cntrlr.GetComponent<Spawner>().Instan();
                Cntrlr.GetComponent<Duplicate>().NextObj();
            }
        }
    }
    void Controller()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0);
            if (!border())
                transform.position += new Vector3(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown((KeyCode.RightArrow)))
        {
            transform.position += new Vector3(1, 0);
            if (!border())
                transform.position += new Vector3(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown((KeyCode.DownArrow)))
        {
            transform.position += new Vector3(0, -1);
            if (!border())
                transform.position += new Vector3(0, 1);
        }
        if (time_sticking())
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0);
                if (!border())
                    transform.position += new Vector3(1, 0);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey((KeyCode.RightArrow)))
            {
                transform.position += new Vector3(1, 0);
                if (!border())
                    transform.position += new Vector3(-1, 0);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                addSpeed = 10;
            }
            else
            {
                addSpeed = 1;
            }
            timer = 0;
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow))
        {
            timer = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(Rotate_point), new Vector3(0, 0, 1), 90);
            if (!border())
            {
                transform.RotateAround(transform.TransformPoint(Rotate_point), new Vector3(0, 0, 1), -90);
            }
        }
    }
    private bool time_sticking()
    {
        if (Input.anyKey)
        {
            timer += Time.deltaTime;
            if (timer < 0.15f)
            {
                return false;
            }
        }
        return true;
    }
    bool border()
    {
        foreach (Transform child in transform)
        {
            int RoundToIntX = Mathf.RoundToInt(child.transform.position.x);
            int RoundToIntY = Mathf.RoundToInt(child.transform.position.y);

            if (RoundToIntX < 0 || RoundToIntX >= 10 ||
                RoundToIntY < 0 ||
                RoundToIntY >= 20)
            {
                return false;
            }

            if (grid[RoundToIntX, RoundToIntY] != null)
            {
                return false;
            }
        }
        return true;
    }
}