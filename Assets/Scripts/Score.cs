using UnityEngine;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject m_loseText;
    [SerializeField] private GameObject m_continue;

    [SerializeField] private KeyCode m_plusKey;
    [SerializeField] private KeyCode m_minusKey;

    [SerializeField] private Text m_scoreText;
    [SerializeField] private Text m_speedText;
    [SerializeField] private Text m_fullLine;

    private float m_speed = 1;

    public float Speed
    {
        get => m_speed;
    }

    public bool add;
    private bool m_lose;

    public bool Lose
    {
        set => m_lose = value;
    }

    private int m_rowMinus;

    public int RowMinus
    {
        get => m_rowMinus;
        set => m_rowMinus = value;
    }

    [SerializeField] private int[] addScore;

    private int m_limitScore = 1;

    private int m_rows;

    public int Rows
    {
        get => m_rows;
        set => m_rows = value;
    }

    private int m_score;
    private int m_currentScore;

    private void Update()
    {
        if (m_lose)
        {
            m_loseText.SetActive(true);
            m_continue.SetActive(true);
            gameObject.GetComponent<Spawner>().enabled = false;
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        m_currentScore = m_limitScore * 1000 - m_score;
        if (m_score >= m_currentScore || Input.GetKeyDown(m_plusKey))
        {
            m_speed = m_speed + 0.1f;
            m_limitScore++;
        }

        if (m_speed > 0)
        {
            if (Input.GetKeyDown(m_minusKey))
            {
                m_speed = m_speed - 0.1f;
            }
        }
        else
            m_speed = 0;

        m_fullLine.text = "The filled lines:\n" + m_rows;
        m_speedText.text = "Speed:\nX" + Mathf.Round(m_speed * 10) / 10;
        m_scoreText.text = "Score:\n" + m_score;
        if (add)
        {
            m_score += addScore[m_rowMinus - 1];
            add = false;
        }
    }
}