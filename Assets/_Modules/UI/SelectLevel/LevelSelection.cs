using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject[] levelObjects; // Mảng các GameObject chứa Button + Stars
    public Sprite starLoseSprite; // Kéo sprite hình sao thua vào trong Inspector

    private void Awake()
    {
        quitButton.onClick.AddListener(() => {
            Loader.Instance.LoadWithFade(SceneName.MainMenuScene);
        });

        for (int i = 0; i < levelObjects.Length; i++)
        {
            int levelIndex = i; // Lưu trữ chỉ số level
            Button levelButton = levelObjects[i].GetComponentInChildren<Button>(); // Lấy Button trong GameObject
            if (levelButton != null)
            {
                levelButton.onClick.AddListener(() => {
                    Player.Instance.currentLevel = levelIndex;
                    Loader.Instance.LoadWithFade(SceneName.GameScene);
                });
            }
        }
    }

    void Start()
    {
        int levelAt = Player.Instance.maxCurrentLevel;
        Debug.Log("levelAt: " + levelAt);
        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (i <= levelAt)
                continue;

            Button levelButton = levelObjects[i].GetComponentInChildren<Button>(); // Lấy Button
            levelButton.interactable = false;

            Image[] starImages = levelObjects[i].GetComponentsInChildren<Image>();

            for (int j = 0; j < 3; j++)
            {
                if (starImages.Length > j + 1) // Đảm bảo không bị lỗi index
                {
                    starImages[j + 1].sprite = starLoseSprite; // <-- Đúng thuộc tính là .sprite
                }
            }
        }
    }
}
