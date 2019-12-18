using UnityEngine;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject Continue;

    [SerializeField] private KeyCode plusKey;
    [SerializeField] private KeyCode minusKey;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text speedText;
    [SerializeField] private Text fullLine;

    [HideInInspector] public float speed = 1;

    [HideInInspector] public bool add;
    [HideInInspector] public bool lose;

    [HideInInspector] public int rowMinus;
    [SerializeField] private int[] addScore;

    private int m_limitScore = 1;

    [HideInInspector] public int rows;
    private int m_score;
    private int m_currentScore;

    private void Update()
    {
        if (lose)
        {
            loseText.SetActive(true);
            Continue.SetActive(true);
            gameObject.GetComponent<Spawner>().enabled = false;
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        m_currentScore = m_limitScore * 1000 - m_score;
        if (m_score >= m_currentScore || Input.GetKeyDown(plusKey))
        {
            speed = speed + 0.1f;
            m_limitScore++;
        }

        if (speed > 0)
        {
            if (Input.GetKeyDown(minusKey))
            {
                speed = speed - 0.1f;
            }
        }
        else
            speed = 0;

        fullLine.text = "The filled lines:\n" + rows;
        speedText.text = "Speed:\nX" + Mathf.Round(speed * 10) / 10;
        scoreText.text = "Score:\n" + m_score;
        if (add)
        {
            m_score += addScore[rowMinus - 1];
            add = false;
        }
    }
}