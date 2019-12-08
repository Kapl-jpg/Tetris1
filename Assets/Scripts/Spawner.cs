using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject parentObj;
    [SerializeField] private GameObject[] tetromino;
    private GameObject[] _blocks;
    public GameObject obj;
    private Vector2 spawnPoint = new Vector2(4, 18);
    private int _random;

    void Start()
    {
        int randomize = Random.Range(0, tetromino.Length);
        obj = Instantiate(tetromino[randomize], spawnPoint, Quaternion.identity);
        obj.transform.parent = parentObj.transform;
    }

    private void FindObj()
    {

    }

    private void Update()
    {
        _blocks = GameObject.FindGameObjectsWithTag("Tetromino");
        for (int i = 0; i < _blocks.Length; i++)
        {
            if (_blocks[i].transform.childCount < 1)
            {
                Destroy(_blocks[i]);
            }
        }

        _random = gameObject.GetComponent<Duplicate>().randomNumber;
    }

    public void InstantiateObj()
    {
        obj = Instantiate(tetromino[_random], spawnPoint, Quaternion.identity);
        obj.transform.parent = parentObj.transform;
        FindObj();
        gameObject.GetComponent<Score>().rowMinus = 0;
    }
}