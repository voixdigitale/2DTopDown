using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Fairy : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;
    [SerializeField] float _tickDelay;
    [SerializeField] GameObject _splatPrefab;

    Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(FairyCoroutine());
    }

    private IEnumerator FairyCoroutine() {
        while (true) {
            yield return new WaitForSeconds(_tickDelay);
            if (!GameManager.instance.IsGameOver) {
                _particles.Play();
                Instantiate(_splatPrefab, transform.position, Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0, 360))));
            }
        }   
    }
}
