using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinGameUI : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject WinPanel;

    private void Awake()
    {
        nextButton.onClick.AddListener(() => {
            Player.Instance.currentLevel++;
            Loader.Instance.LoadWithFade(SceneName.GameScene);
        });
        replayButton.onClick.AddListener(() => Loader.Instance.LoadWithFade(SceneName.GameScene));
        homeButton.onClick.AddListener(() => Loader.Instance.LoadWithFade(SceneName.MainMenuScene));

        WinPanel.SetActive(false);
    }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnWinGame>(OnWinGame);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnWinGame>(OnWinGame);
    }

    private void OnWinGame(IEventParam param)
    {
        Debug.Log("WIN");
        AudioManager.Instance.PlaySound(GameAudioClip.REWARD_SOUND);

        if (Player.Instance.currentLevel + 1 == GameManager.Instance.levelPrefabs.Length)
            nextButton.interactable = false;
        WinPanel.SetActive(true);
    }
}