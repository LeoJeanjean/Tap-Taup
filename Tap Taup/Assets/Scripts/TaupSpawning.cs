using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaupSpawning : MonoBehaviour
{
    public static TaupSpawning Instance { get; private set; }

    [SerializeField] private GameObject[] taupPrefab;
    [SerializeField] private Transform[] spawnPoints;
    public Dictionary<string, Vector3> spawnedTaupPositions;

    private HashSet<Transform> occupiedSpawnPoints;

    private void Awake()
    {
        Instance = this;
        spawnedTaupPositions = new Dictionary<string, Vector3>();
        occupiedSpawnPoints = new HashSet<Transform>();

        foreach (GameObject taup in taupPrefab)
        {
            ObjectPooling.Instance.AddPoolObject(taup.name, taup);
            spawnedTaupPositions[taup.name] = Vector3.zero;
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnTaup());
    }

    private IEnumerator SpawnTaup()
    {
        while (true)
        {
            Transform spawnPoint = GetAvailableSpawnPoint();
            if (spawnPoint != null)
            {
                GameObject? taup = TryGetTaup();
                if (taup == null)
                {
                    yield return new WaitForSeconds(GameManager.Instance.spawnRate);
                    continue;
                }
                taup.transform.position = spawnPoint.position;
                spawnedTaupPositions[taup.name] = taup.transform.position;
                occupiedSpawnPoints.Add(spawnPoint);

                taup.SetActive(true);
                taup.GetComponent<Taup>().Activate();
            }

            yield return new WaitForSeconds(GameManager.Instance.spawnRate);
        }
    }

    private GameObject? TryGetTaup()
    {
        int numberOfTries = 0;
        while (numberOfTries < taupPrefab.Length)
        {
            int taupIndex = Random.Range(0, taupPrefab.Length);
            string taupName = taupPrefab[taupIndex].name;

            if (spawnedTaupPositions.ContainsKey(taupName) &&
                spawnedTaupPositions[taupName] == Vector3.zero)
            {
                return ObjectPooling.Instance.GetPooledObject(taupName);
            }
            numberOfTries++;
        }
        return null;
    }

    private Transform GetAvailableSpawnPoint()
    {
        List<Transform> freePoints = new List<Transform>();
        foreach (Transform point in spawnPoints)
        {
            if (!occupiedSpawnPoints.Contains(point)) freePoints.Add(point);
        }

        return freePoints.Count > 0 ? freePoints[Random.Range(0, freePoints.Count)] : null;
    }

    public void DeactivateTaup(GameObject taup)
    {
        ObjectPooling.Instance.AddPoolObject(taup.name, taup);
        spawnedTaupPositions[taup.name] = Vector3.zero;


        Transform occupiedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (occupiedSpawnPoints.Contains(occupiedPoint))
        {
            occupiedSpawnPoints.Remove(occupiedPoint);
        }

        taup.transform.position = Vector3.zero;
    }
}
