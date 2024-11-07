using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Taup : MonoBehaviour
{
    private Vector3 groundPosition;

    private Coroutine moveToGroundCoroutine;

    public void Activate()
    {
        // Set the target position above the ground
        Vector3 targetPosition = transform.position;
        groundPosition = transform.position + Vector3.down * 3;
        transform.position = groundPosition;
        transform.LookAt(GameManager.Instance.player.transform);
        // Move down in the ground
        transform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad);
        moveToGroundCoroutine = StartCoroutine(MoveToGround());
    }

    public IEnumerator MoveToGround()
    {
        yield return new WaitForSeconds(GameManager.Instance.taupDuration);
        DOTween.Sequence()
            .Append(transform.DOMove(groundPosition, 1f).SetEase(Ease.InQuad))
            .AppendCallback(() => Deactivate())
            .Play();
    }

    public void Kill()
    {
        StopCoroutine(moveToGroundCoroutine);
        TaupSpawning.Instance.DeactivateTaup(gameObject);
        GameManager.Instance.score += GameManager.Instance.combo > 0 ? GameManager.Instance.combo: 1;
        GameManager.Instance.combo++;
    }

    public void Deactivate()
    {
        GameManager.Instance.combo = 0;
        // GameManager.Instance.playerHealth--;
        TaupSpawning.Instance.DeactivateTaup(gameObject);
    }
}
