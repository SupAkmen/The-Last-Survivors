using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota; // tong so ke thu de sinh ra trong dot
        public float spawnInterval; // khoang thoi gian xuat hien moi dot ke thu
        public int spawnCount; // so luong ke thu da sinh ra trong dot
    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // so luong ke thu thuoc loai nay xh trong dot
        public int spawnCount; // so luong ke thu thuoc loai nay da xh trong dot
        public GameObject enemyPrefabs;

    }

    public List<Wave> waves;
    public int currentWaveCount; // the index of the current wave
    public int totalEnemyKill;
    private const string totalEnemyKillKey = "TotalEnemyKill";

    [Header("Spawner Attributtes")]
    float spawnTimer; // xac dinh luot quai tiep theo dc sinh ra
    public int enemiesAlive; // so lg ke thu dang song
    public float maxEnemiesAllowed; // slg ke thu duoc phep tren map
    public bool maxEnemiesReached = false; // ktra so lg ke thu da dat toi da hay chua
    public float waveInterval; // khoang thoi gian giua cac dot xh
    bool isWaveActive= false;

    [Header("Spawn Position")]
    public List<Transform> relativeSpawnPoints; // cac vi tri san sinh ra ke thu

    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();

    }

    private void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true;
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive= false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }
    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
    }

    void SpawnEnemies()
    {
        // sinh ra cac dot quai neu so lg quai trong dot chua het
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            // lap lai so luong quai trong nhom quai
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // neu so luong quai sinh ra chua max
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Instantiate(enemyGroup.enemyPrefabs, player.position + relativeSpawnPoints[Random.Range(0,relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    // gioi han so lg quai co the sinh ra trong luot
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
    }

    public void OnEnemyKilled()
    {
        totalEnemyKill = PlayerPrefs.GetInt(totalEnemyKillKey, 0);
        totalEnemyKill++;
        PlayerPrefs.SetInt(totalEnemyKillKey, totalEnemyKill);
        enemiesAlive--;

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}
