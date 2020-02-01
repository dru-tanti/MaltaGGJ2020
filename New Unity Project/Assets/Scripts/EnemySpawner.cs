using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to hold the enemy data.
[System.Serializable]
public class EnemyType {
    public string enemyName;
    public GameObject enemyPrefab;
}

public class EnemySpawner : MonoBehaviour {
    public EnemyType[] enemies;
    
    // Spawns an enemy and adds them as a reference in the SpawnManager
    public void SpawnEnemy() {
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)].enemyPrefab, transform.position, Quaternion.identity);
        EnemySpawnManager.current.enemies.Add(enemy);
    }
}
