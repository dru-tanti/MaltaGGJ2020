using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to hold the enemy data.
[System.Serializable]
public class EnemyType {
    public string enemyName;
    public GameObject enemyPrefab;
    [Range(0f, 100f)]
    public int spawnChance;
}

public class EnemySpawner : MonoBehaviour {
    public EnemyType[] enemies;
    [Range(0f, 10f)]
    [Tooltip("Rate that the spawner will spawn enemies in seconds")]
    public float spawnRate;
    
    // Spawns an enemy and adds them as a reference in the SpawnManager
    public void SpawnEnemy() {
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)].enemyPrefab, transform.position, Quaternion.identity);
        EnemySpawnManager.current.enemies.Add(enemy);
    }
}
