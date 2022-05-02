using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    [HideInInspector]
    public bool StartTimer = false;

    // SET THIS TO DETERMINE GAME LENGTH (SECONDS)
    private const float GameLength = 60;

    private float TimeRemaining = GameLength;

    private void Update() {
        if (StartTimer) {
            if (TimeRemaining > 0) {
                TimeRemaining -= Time.deltaTime;
            } else if (TimeRemaining == 20) {
                // Crazy nut time mode!
            } else {
                Debug.Log("Game is over");
                TimeRemaining = 0;
                StartTimer = false;
            }
        }
    }

    public string GetTimeString() {
        float minutes = Mathf.FloorToInt(TimeRemaining / 60);
        float seconds = Mathf.FloorToInt(TimeRemaining % 60);

        if (minutes == 0.0f) {
            return string.Format("{00}", seconds);
        }

        return string.Format("{00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer() => TimeRemaining = GameLength;
}
