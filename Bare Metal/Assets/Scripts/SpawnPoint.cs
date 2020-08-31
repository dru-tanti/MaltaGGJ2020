using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public EnemySpawner spawner;
    // Spawns an enemy and adds them as a reference in the SpawnManager
    public void SpawnEnemy() {
        GameObject enemy = Instantiate(spawner.enemies[Random.Range(0, spawner.enemies.Length)], transform.position, Quaternion.identity);
        EnemySpawnManager.current.enemies.Add(enemy);
    }
}
