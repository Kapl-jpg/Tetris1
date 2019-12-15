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
    public float speed = 1;

    public bool add;
    public bool lose;

    public int rowMinus;
    [SerializeField] private int[] addScore;

    private int _limitScore = 1;

    public int rows;
    private int _score;
    private int _currentScore;

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

        _currentScore = _limitScore * 1000 - _score;
        if (_score >= _currentScore || Input.GetKeyDown(plusKey))
        {
            speed = speed + 0.1f;
            _limitScore++;
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
        scoreText.text = "Score:\n" + _score;
        if (add)
        {
            _score += addScore[rowMinus - 1];
            add = false;
        }
    }
}