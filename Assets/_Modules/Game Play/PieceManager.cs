using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public List<Transform> pieces;            // Danh sách các mảnh
    public float moveStep = 1.0f;             // Mỗi lần di chuyển 1 ô
    public float tweenDuration = 0.2f;        // Thời gian tween
    public LayerMask obstacleLayer;           // Layer vật cản (block, tường...)
    public LayerMask playerLayer;             // Layer các mảnh (Player)

    private Vector2 swipeStart;

    void Start()
    {
        StartCoroutine(ShufflePieces());
    }

    IEnumerator ShufflePieces()
    {
        int random = Random.Range(10, 15);

        for (int i = 0; i < random; i++)
        {
            MoveShufflePieces();
            yield return new WaitForSeconds(tweenDuration);
        }
    }

    void MoveShufflePieces()
    {
        HashSet<Vector2> reservedPositions = new HashSet<Vector2>(); // lưu vị trí đã chọn

        foreach (Transform piece in pieces)
        {
            BoxCollider2D box = piece.GetComponent<BoxCollider2D>();
            if (box == null)
            {
                Debug.LogWarning($"{piece.name} chưa có BoxCollider2D");
                continue;
            }

            Vector2 currentPos = piece.position;
            Vector2 checkSize = box.bounds.size * 0.5f;

            Vector2 direction = GetRandomDirection();
            Vector2 targetPos = currentPos + direction * moveStep;

            // Kiểm tra có va chạm chỗ targetPos chưa?
            Collider2D hit = Physics2D.OverlapBox(targetPos, checkSize, 0f, obstacleLayer | playerLayer);

            // Kiểm tra xem vị trí này đã có piece nào dự định di chuyển tới chưa
            bool isReserved = reservedPositions.Contains(targetPos);

            if (hit == null && !isReserved)
            {
                // Cho phép di chuyển
                piece.DOMove(targetPos, tweenDuration).SetEase(Ease.OutQuad);
                reservedPositions.Add(targetPos); // đặt vị trí này là đã dùng
            }
            // Nếu có va chạm hoặc đã bị đặt rồi → không di chuyển
        }
    }


    Vector2 GetRandomDirection()
    {
        Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        return directions[Random.Range(0, directions.Length)];
    }


    void Update()
    {
        HandleSwipe();
    }

    void HandleSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeEnd = Input.mousePosition;
            Vector2 swipeDelta = swipeEnd - swipeStart;

            if (swipeDelta.magnitude > 30f)
            {
                Vector2 direction = Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)
                    ? (swipeDelta.x > 0 ? Vector2.right : Vector2.left)
                    : (swipeDelta.y > 0 ? Vector2.up : Vector2.down);

                MoveAllPieces(direction);
            }
        }
    }

    void MoveAllPieces(Vector2 direction)
    {
        foreach (Transform piece in pieces)
        {
            BoxCollider2D box = piece.GetComponent<BoxCollider2D>();
            if (box == null)
            {
                Debug.LogWarning($"{piece.name} chưa có BoxCollider2D");
                continue;
            }

            Vector2 currentPos = piece.position;
            Vector2 checkSize = box.bounds.size * 0.5f;
            Vector2 checkPos = currentPos + direction * moveStep;

            bool canMove = false;

            while (true)
            {
                Collider2D hit = Physics2D.OverlapBox(checkPos, checkSize, 0f, obstacleLayer | playerLayer);

                if (hit == null)
                {
                    // Ô trống, có thể di chuyển ở vị trí checkPos
                    canMove = true;
                    break;
                }

                int hitLayerMask = 1 << hit.gameObject.layer;

                if ((hitLayerMask & obstacleLayer) != 0)
                {
                    // Gặp vật cản → không di chuyển
                    canMove = false;
                    break;
                }

                if ((hitLayerMask & playerLayer) != 0)
                {
                    // Gặp player → kiểm tra ô kế tiếp
                    checkPos += direction * moveStep;
                    continue;
                }
            }

            if (canMove)
            {
                // Di chuyển đúng 1 đơn vị (moveStep) theo direction, không phải đến checkPos
                Vector2 targetPos = currentPos + direction * moveStep;
                piece.DOMove(targetPos, tweenDuration).SetEase(Ease.OutQuad);
            }
            // Nếu không thể di chuyển thì giữ nguyên vị trí
        }
    }

}
