using System.Collections;
using UnityEngine;

public class PiecesChecker : MonoBehaviour
{
    public GameObject piece;          // Mảnh gốc để kiểm tra
    public float moveStep = 1f;       // Khoảng cách bước di chuyển giữa các mảnh
    public float checkInterval = 0.5f; // Kiểm tra mỗi 0.5s
    public float timeLimit = 45f;     // Giới hạn thời gian

    public float timer = 0f;
    public bool gameEnded = false;

    private void Start()
    {
        StartCoroutine(CheckWinLoseRoutine());
    }

    IEnumerator CheckWinLoseRoutine()
    {
        yield return new WaitForSeconds(5f);

        timer = 0f;
        gameEnded = false;

        while (timer < timeLimit && !gameEnded)
        {
            if (CheckPiecesArrangement())
            {
                gameEnded = true;
                Debug.Log("Bạn đã chiến thắng!");
                EventDispatcher.Dispatch(new EventDefine.OnWinGame());
                yield break;
            }

            timer += checkInterval;
            yield return new WaitForSeconds(checkInterval);
        }

        if (!gameEnded)
        {
            gameEnded = true;
            Debug.Log("Bạn đã thua!");
            EventDispatcher.Dispatch(new EventDefine.OnLoseGame());
        }
    }

    bool CheckPiecesArrangement()
    {
        Vector2 basePos = piece.transform.position;

        GameObject up = GetPieceAtPosition(basePos + Vector2.up * moveStep);
        GameObject left = GetPieceAtPosition(basePos + Vector2.right * moveStep);
        GameObject upLeft = GetPieceAtPosition(basePos + (Vector2.up + Vector2.right) * moveStep);

        if (up == null || up.name != "orange 3_0") return false;
        if (left == null || left.name != "orange 2_0") return false;
        if (upLeft == null || upLeft.name != "orange 4_0") return false;

        return true;
    }


    GameObject GetPieceAtPosition(Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        if (hit != null && hit.gameObject.CompareTag("Player"))
        {
            return hit.gameObject;
        }
        return null;
    }
}
