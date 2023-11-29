using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }
    public bool IsGameOver;

    private Vector2 _startingPos = new Vector2(-4f, .5f);
    private Vector2 _startingFairyPos = new Vector2(-6f, -2f);

    [SerializeField] GameObject _pressKeyPanel;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _victoryPanel;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] int _timerSeconds = 120;
    [SerializeField] Tilemap _treasureTilemap;
    [SerializeField] Tile _treasureTile;
    [SerializeField] List<Vector2> _possibleSpawnPoints;
    [SerializeField] Transform _player;
    [SerializeField] Transform _fairy;

    private bool _treasureFound;
    private bool _treasureTouched;
    private bool _paused = false;
    int _timer;

    void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    private void OnEnable() {
        TreasureFinder.OnTreasureFind += OnFindTreasure;
        TreasureFinder.OnTreasureTouch += OnTouchTreasure;
    }

    private void OnDisable() {
        TreasureFinder.OnTreasureFind -= OnFindTreasure;
        TreasureFinder.OnTreasureTouch -= OnTouchTreasure;
    }

    private void Start() {
        IsGameOver = true;
        _pressKeyPanel.SetActive(true);
    }

    private void SetupGame() {
        IsGameOver = true;
        _treasureFound = _treasureTouched = false;
        _timer = _timerSeconds;
        _player.position = _startingPos;
        _fairy.position = _startingFairyPos;
        _fairy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Splat");
        foreach(GameObject g in toDestroy) {
            Destroy(g);
        }
        HideTreasure();
        StartGame();
    }

    private void StartGame() {
        IsGameOver = false;
        _pressKeyPanel.SetActive(false);
        _victoryPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        StartCoroutine(StartTimer());
    }

    private void Update() {
        if (IsGameOver && !_paused && Input.anyKey) {
            SetupGame();
        }

        if (!IsGameOver && _treasureFound && _treasureTouched) {
            IsGameOver = true;
            _paused = true;
            _victoryPanel.SetActive(true);
        }

        if(_paused) {
            StartCoroutine(UnPause());
        }

    }

    private void HideTreasure() {
        Vector2 selectedPos = _possibleSpawnPoints[Random.Range(0, _possibleSpawnPoints.Count)];
        _treasureTilemap.SetTile(new Vector3Int((int) selectedPos.x, (int) selectedPos.y), _treasureTile);
    }

    private IEnumerator StartTimer() {
        if (!IsGameOver) {
            yield return new WaitForSeconds(1f);
            _timer--;
            string format = "00";
            int minutes = _timer / 60;
            int seconds = _timer % 60;
            _timerText.text = $"Timer {minutes}:{seconds.ToString(format)}";

            if (_timer > 0) {
                StartCoroutine(StartTimer());
            } else {
                IsGameOver = true;
                _paused = true;
                _gameOverPanel.SetActive(true);
            }
        }
    }

    private void OnFindTreasure() {
        _treasureFound = true;
    }
    
    private void OnTouchTreasure() {
        if (_treasureFound)
            _treasureTouched = true;
    }

    private IEnumerator UnPause() {
        yield return new WaitForSeconds(2f);
        _paused = false;
    }
}
