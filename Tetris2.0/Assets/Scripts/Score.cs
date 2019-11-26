using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public GameObject Lose_text;
    public GameObject Continue;
    public KeyCode pl_key;
    public KeyCode mn_key;
    public Text scoreText;
    public Text speedText;
    public Text fullLine;
    public float speed = 1;
    public int rows;
    private int score;
    private int currentScore;
    public bool add;
    public bool Lose;
    public int row;
    public int i = 1;

    void Update()
    {
        if (Lose)
        {
            Lose_text.SetActive(true);
            Continue.SetActive(true);
            gameObject.GetComponent<Spawner>().enabled = false;
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        currentScore = (i) * 1000 - score;
        if (score >= currentScore || Input.GetKeyDown(pl_key))
        {
            speed = speed + 0.1f;
            i++;
        }

        if (speed > 0)
        {
            if (Input.GetKeyDown(mn_key))
            {
                speed = speed - 0.1f;
            }
        }
        else
            speed = 0;

        fullLine.GetComponent<Text>().text = "The filled lines: " + rows;
        speedText.GetComponent<Text>().text = "Speed: X" + Mathf.Round((float)speed*10)/10;
        scoreText.GetComponent<Text>().text = "Score: " + Mathf.RoundToInt(score);
        if (add)
        {
            if (row == 1)
                score += 100;
            else if (row == 2)
                score += 200;
            else if (row == 3)
                score += 400;
            else if (row == 4)
                score += 900;
            add = false;
        }
        else
        {
            row = 0;
        }
    }
}