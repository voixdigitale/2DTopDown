using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextColorChanger : MonoBehaviour
{
    [SerializeField] Color _color1 = Color.white;
    [SerializeField] Color _color2 = Color.red;
    [SerializeField] float _interval = 1.0f;

    TextMeshProUGUI _text;
    float _timeToChange;

    private void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Start() {
        _text.color = _color1;
        _timeToChange = Time.time + _interval;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time >= _timeToChange) {
            _text.color = _text.color == _color1 ? _color2 : _color1;
            _timeToChange = Time.time + _interval;
        }
    }
}
