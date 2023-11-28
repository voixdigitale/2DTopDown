using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureFinder : MonoBehaviour
{
    public static Action OnTreasureFind;
    public static Action OnTreasureTouch;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Splat")) {
            OnTreasureFind?.Invoke();
        } else if (collision.gameObject.CompareTag("Player")) {
            OnTreasureTouch?.Invoke();
        }
    }
}
