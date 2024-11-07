using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Taup : MonoBehaviour
{
    [HideInInspector] public Transform usingSpawnPoint = null;

    private Vector3 groundPosition;

    private Coroutine moveToGroundCoroutine;

    private Sequence currentSequence;

    public void Activate()
    {
        // Set the target position above the ground
        Vector3 targetPosition = transform.position;
        groundPosition = transform.position + Vector3.down * 3;
        transform.position = groundPosition;
        transform.LookAt(GameManager.Instance.player.transform);
        // Move down in the ground
        transform.DOMove(targetPosition, 1f*GameManager.Instance.spawnRate).SetEase(Ease.OutQuad);
        moveToGroundCoroutine = StartCoroutine(MoveToGround());
    }

    public IEnumerator MoveToGround()
    {
        yield return new WaitForSeconds(GameManager.Instance.taupDuration);
        currentSequence = DOTween.Sequence()
            .Append(transform.DOMove(groundPosition, 1f*GameManager.Instance.spawnRate).SetEase(Ease.InQuad))
            .AppendCallback(() => Deactivate())
            .Play();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hammer"))
        {
            currentSequence.Kill();
            GameManager.Instance.sfxAudioSource.PlayOneShot(GameManager.Instance.moleHits[Random.Range(0, GameManager.Instance.moleHits.Count)]);
            StopCoroutine(moveToGroundCoroutine);
            TaupSpawning.Instance.DeactivateTaup(gameObject);
            GameManager.Instance.score += GameManager.Instance.combo*100;
            GameManager.Instance.combo++;
            GameManager.Instance.uiManager.UpdateGUI(ValueType.combo, GameManager.Instance.combo);
            GameManager.Instance.uiManager.UpdateGUI(ValueType.score, GameManager.Instance.score);
        }
    }

    public void Deactivate()
    {
        if(GameManager.gameStarted)
        {
            GameManager.Instance.sfxAudioSource.PlayOneShot(GameManager.Instance.heartLost);
            GameManager.Instance.SetDifficultyBack();
            GameManager.Instance.uiManager.UpdateGUI(ValueType.combo, 0);
            GameManager.Instance.uiManager.UpdateGUI(ValueType.health, 1);
            GameManager.Instance.combo = 1;
            GameManager.Instance.playerHealth--;
            Debug.Log("zzzz reduced player health; is now "+GameManager.Instance.playerHealth);
            TaupSpawning.Instance.DeactivateTaup(gameObject);
        }
    }
}
