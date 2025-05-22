using UnityEngine;
using TMPro;

public class TMP_TimerDisplay : MonoBehaviour
{
    public TMP_Text timerText; // Kéo TMP Text vào đây trong Inspector

    private float timer = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;

            int totalSeconds = Mathf.FloorToInt(timer);
            totalSeconds = Mathf.Clamp(totalSeconds, 0, 45);

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

            if (totalSeconds >= 45)
            {
                isRunning = false;
            }
        }
    }
}
