using DG.Tweening;
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
