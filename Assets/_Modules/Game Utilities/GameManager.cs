using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject[] levelPrefabs; // Prefab của Level
    private GameObject currentLevelInstance; // Instance của Level hiện tại

    private void Start()
    {
        LoadLevel(Player.Instance.currentLevel);
        TimeRun();
    }

    public void TimeRun()
    {
        Time.timeScale = 1;
    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }

    public void LoadLevel(int levelIndex)
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance); // Xóa Level cũ nếu có
        }

        if (levelIndex < 0 || levelIndex >= levelPrefabs.Length)
        {
            Debug.LogError("Level index out of range: " + levelIndex);
            return;
        }

        Debug.Log("Loading level: " + levelIndex);
        Player.Instance.currentLevel = levelIndex;

        // Tạo level mới từ Prefab
        currentLevelInstance = Instantiate(levelPrefabs[Player.Instance.currentLevel], transform.position, Quaternion.identity);
        currentLevelInstance.transform.parent = GameObject.Find("Environment").transform;
    }
}
