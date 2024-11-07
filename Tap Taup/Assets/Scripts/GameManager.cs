using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool gameStarted = false;

    [Header("Gameplay settings")]
    [SerializeField] public GameObject player;
    [SerializeField] private TaupSpawning taupSpawning;
    private float baseTaupDuration;
    [SerializeField] public float taupDuration = 3f;
    [SerializeField] private float taupDurationMin = 0.5f;
    private float baseSpawnRate;
    [SerializeField] private float spawnRateMin = 0.5f;
    [SerializeField] public float spawnRate = 1f; // Delay between taup spawns
    [SerializeField] private float augmentationRate = 0.005f; // augmentation rate of the spawn rate per second
    public float difficultSetbackOnHit = 1;

    [Header("Player scoring")]
    [ReadOnly(true)] public int playerHealth = 3;
    [ReadOnly(true)] public int score = 0;
    [ReadOnly(true)] public int combo = 0;

    [Header("Hammer loss prevention")]
    public Transform hammer;
    private Vector3 hammerStartPosition;

    [Header("UI handling")]
    public UIManager uiManager;

    [Header("Audio")]
    public AudioSource sfxAudioSource;
    public List<AudioClip> moleHits;
    public AudioClip heartLost;
    public AudioClip gameOver;


    void Awake()
    {
        Instance = this;
        hammerStartPosition = hammer.position;

        baseTaupDuration = taupDuration;
        baseSpawnRate = spawnRate;
    }

    public void StartGame()
    {
        gameStarted = true;

        taupDuration = baseTaupDuration;
        spawnRate = baseSpawnRate;

        uiManager.ShowAndResetGameMenu();
        taupSpawning.StartSpawning();
        StartCoroutine(AugmentDifficulty());
    }
    public void EndGame()
    {
        gameStarted = false;
        
        uiManager.SelectMenu(MenuSelector.end);
        uiManager.finalScoreText.text = score.ToString();

        playerHealth = 3;
        score = 0;
        combo = 1;

        //taupSpawning.StopSpawning();
        sfxAudioSource.Stop();
        sfxAudioSource.PlayOneShot(gameOver);
        StopCoroutine(AugmentDifficulty());
        Debug.Log("ddd occupiedSpawnPoints count = "+taupSpawning.occupiedSpawnPoints.Count);
        taupSpawning.occupiedSpawnPoints.Clear();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void OnTriggerExit(Collider other)
    {
        Debug.Log("AAAAAAAAAAAA");
        if(other.CompareTag("Hammer"))
        {
            Debug.Log("EBBBBBBBBBBBBB");
            hammer.position = hammerStartPosition;
            hammer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
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
    public void SetDifficultyBack()
    {
        spawnRate += difficultSetbackOnHit;
        taupDuration += difficultSetbackOnHit;
    }

    void Update()
    {
        if (playerHealth <= 0)
        {
            EndGame();
        }
    }
}
