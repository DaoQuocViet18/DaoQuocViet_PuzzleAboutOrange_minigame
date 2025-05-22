using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseGameUI : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private GameObject LosePanel;

    private void Awake()
    {
        replayButton.onClick.AddListener(() => Loader.Instance.LoadWithFade(SceneName.GameScene));
        homeButton.onClick.AddListener(() => Loader.Instance.LoadWithFade(SceneName.MainMenuScene));

        LosePanel.SetActive(false);
    }

    private void OnEnable()
    {
        // Đăng ký sự kiện OnLoseGame
        EventDispatcher.Add<EventDefine.OnLoseGame>(OnLoseGame);
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị hủy
        EventDispatcher.Remove<EventDefine.OnLoseGame>(OnLoseGame);
    }

    // Xử lý sự kiện OnLoseGame
    private void OnLoseGame(IEventParam param)
    {
        Debug.Log("Game Over! LosePanel is now active.");
        AudioManager.Instance.PlaySound(GameAudioClip.GAMEOVER_SOUND);
        LosePanel.SetActive(true); // Hiển thị LosePanel
    }
}
