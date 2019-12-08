using UnityEngine;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject loseText, Continue;
    [SerializeField] private KeyCode plusKey, minusKey;
    [SerializeField] private Text scoreText, speedText, fullLine;
    public float speed = 1;

    public bool add;
    public bool lose;

    public int rowMinus;
    private int _granicaScore = 1;

    public int rows;
    private int _score;
    private int _currentScore;

    void Update()
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

        _currentScore = _granicaScore * 1000 - _score;
        if (_score >= _currentScore || Input.GetKeyDown(plusKey))
        {
            speed = speed + 0.1f;
            _granicaScore++;
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
            switch (rowMinus)
            {
                case 0:
                    _score += 0;
                    break;
                case 1:
                    _score += 100;
                    break;
                case 2:
                    _score += 200;
                    break;
                case 3:
                    _score += 400;
                    break;
                case 4:
                    _score += 900;
                    break;
            }

            add = false;
        }
    }
}