using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EventDefine;

public class PlayGameUI : MonoBehaviour
{
    public TMP_Text timerText; // Kéo TMP Text vào đây trong Inspector
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;

    private float timer = 0f;
    private bool isRunning = true;

    private void Awake()
    {
        replayButton.onClick.AddListener(() => Loader.Instance.LoadWithFade(SceneName.GameScene));
        homeButton.onClick.AddListener(() => Loader.Instance.LoadWithFade(SceneName.MainMenuScene));
    }

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

    private void OnEnable()
    {
        // Đăng ký sự kiện OnLoseGame
        EventDispatcher.Add<EventDefine.OnLoseGame>(OnLoseGame);
        EventDispatcher.Add<EventDefine.OnWinGame>(OnWinGame);
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị hủy
        EventDispatcher.Remove<EventDefine.OnLoseGame>(OnLoseGame);
        EventDispatcher.Remove<EventDefine.OnWinGame>(OnWinGame);

    }

    // Xử lý sự kiện OnLoseGame
    private void OnLoseGame(IEventParam param)
    {
        isRunning = false;
    }

    private void OnWinGame(IEventParam param)
    {
        isRunning = false;
    }
}
