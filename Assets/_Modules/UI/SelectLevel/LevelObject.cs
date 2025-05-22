using UnityEngine;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    public Button levelButton;
    public Image[] stars; // Mảng chứa 3 ngôi sao

    private void Awake()
    {
        // Nếu chưa assign button trong Inspector
        if (levelButton == null)
        {
            levelButton = GetComponentInChildren<Button>();
        }

        // Nếu chưa assign stars trong Inspector
        if (stars == null || stars.Length == 0)
        {
            stars = GetComponentsInChildren<Image>();
        }
    }
}
