using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region System methods

    private void Awake()
    {
        ObjectPool.PreLoadPool(_ballPrefab, 5);
        ObjectPool.PreLoadPool(_cubePrefab, 20);

        Init();

        BallLauncher player = FindObjectOfType<BallLauncher>();
        if (player != null)
            player.NewPlayerTurnEvent += OnNewPlayerTurn;
    }

    #endregion


    #region Methods

    private void Init()
    {
        _score = 0;

        for (int i = 0; i < 4; i++)
            SpawnCubeRow(4 + (i * -1.1f));

        UpdateUI();
    }

    private void OnNewPlayerTurn()
    {
        if (_cubes.Count > 0)
        {
            MoveAllCubesDown();
            SpawnCubeRow(4f);
        }
        else
        {
            // Game won
            // You can invoke an event and handle your game winning screens, ...
        }
    }

    private void OnCubeDeath(Cube cube)
    {
        _cubes.Remove(cube);
    }

    private void OnScoreGained()
    {
        _score++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _scoreText.text = _score.ToString();
    }

    private void SpawnCubeRow(float y)
    {
        for (int i = 0; i < _rowWidth; i++)
        {
            if (Random.Range(0, 100) <= 30)
            {
                Cube cube = ObjectPool.GetFromPool(_cubePrefab).GetComponent<Cube>();
                cube.Transform.position = new Vector2(_firstCubeSpawnX + (i * _distanceBetweenCube), y);
                cube.InitCube(Random.Range(1, 10));

                cube.CubeDeathEvent += OnCubeDeath;
                cube.ScoreGainedEvent += OnScoreGained;

                _cubes.Add(cube);
            }
        }
    }

    private void MoveAllCubesDown()
    {
        foreach (Cube cube in _cubes)
        {
            if (cube != null)
                cube.Transform.position = new Vector2(cube.Transform.position.x, cube.Transform.position.y - 1.1f);
        }
    }

    #endregion


    #region Private fields

    [SerializeField] private GameObject _ballPrefab = null;
    [SerializeField] private GameObject _cubePrefab = null;

    [SerializeField] private float _firstCubeSpawnX = -2.2f;
    [SerializeField] private int _rowWidth = 4;
    [SerializeField] private float _distanceBetweenCube = 1.1f;

    [SerializeField] private TextMeshProUGUI _scoreText = null;

    private int _score = 0;

    private List<Cube> _cubes = new List<Cube>();

    #endregion
}
