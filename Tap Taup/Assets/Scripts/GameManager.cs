using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake() => Instance = this;
    [SerializeField] public GameObject player;
    [SerializeField] private TaupSpawning taupSpawning;
    [SerializeField] public float taupDuration = 3f;
    [SerializeField] private float taupDurationMin = 0.5f;
    [SerializeField] private float spawnRateMin = 0.5f;
    [SerializeField] public float spawnRate = 1f; // Delay between taup spawns
    [SerializeField] private float augmentationRate = 0.005f; // augmentation rate of the spawn rate per second

    public int playerHealth = 300;
    public int score = 0;
    public int combo = 0;

    private void Start()
    {
        taupSpawning.StartSpawning();
        StartCoroutine(AugmentDifficulty());
    }

    private IEnumerator AugmentDifficulty()
    {
        while (playerHealth > 0)
        {
            yield return new WaitForSeconds(1f);
            if (spawnRate > spawnRateMin)
                spawnRate -= augmentationRate;
            if (taupDuration > taupDurationMin)
                taupDuration -= augmentationRate;
        }
    }

    void Update()
    {
        if (playerHealth <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}
