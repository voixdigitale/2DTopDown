using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject _pressKeyPanel;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _victoryPanel;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] int _timerSeconds = 120;
    [SerializeField] Tilemap _treasureTilemap;
    [SerializeField] Tile _treasureTile;
    [SerializeField] List<Vector2> _possibleSpawnPoints;

    private bool _isGameOver;
    private bool _treasureFound;
    private bool _treasureTouched;
    int _timer;

    private void OnEnable() {
        TreasureFinder.OnTreasureFind += OnFindTreasure;
        TreasureFinder.OnTreasureTouch += OnTouchTreasure;
    }

    private void OnDisable() {
        TreasureFinder.OnTreasureFind -= OnFindTreasure;
        TreasureFinder.OnTreasureTouch -= OnTouchTreasure;
    }

    private void Start() {
        SetupGame();
        _pressKeyPanel.SetActive(true);
    }

    private void SetupGame() {
        Time.timeScale = 0;
        _isGameOver = true;
        _treasureFound = _treasureTouched = false;
        _timer = _timerSeconds;
        HideTreasure();
    }

    private void StartGame() {
        _isGameOver = false;
        Time.timeScale = 1;
        _pressKeyPanel.SetActive(false);
        _victoryPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        StartCoroutine(StartTimer());
    }

    private void Update() {
        if (_isGameOver && Input.anyKey) {
            StartGame();
        }

        if (!_isGameOver && _treasureFound && _treasureTouched) {
            _isGameOver = true;
            Time.timeScale = 0;
            _victoryPanel.SetActive(true);
        }

    }

    private void HideTreasure() {
        Vector2 selectedPos = _possibleSpawnPoints[Random.Range(0, _possibleSpawnPoints.Count)];
        _treasureTilemap.SetTile(new Vector3Int((int) selectedPos.x, (int) selectedPos.y), _treasureTile);
    }

    private IEnumerator StartTimer() {
        yield return new WaitForSeconds(1f);
        _timer--;
        string format = "00";
        int minutes = _timer / 60;
        int seconds = _timer % 60;
        _timerText.text = $"Timer {minutes}:{seconds.ToString(format)}";

        if (_timer > 0) {
            StartCoroutine(StartTimer());
        } else {
            _isGameOver = true;
            Time.timeScale = 0;
            _gameOverPanel.SetActive(true);
        }
    }

    private void OnFindTreasure() {
        _treasureFound = true;
    }
    
    private void OnTouchTreasure() {
        if (_treasureFound)
            _treasureTouched = true;
    }
}
