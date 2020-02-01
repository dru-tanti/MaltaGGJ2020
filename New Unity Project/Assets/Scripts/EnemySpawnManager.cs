using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    public GameObject[] spawners; // Reference for all the spawners in the scene.
    public List<GameObject> enemies = new List<GameObject>(); // Keeps a reference of all the spawned enemies currently in the scene.
    [Range(0, 20)]
    public int enemyLimit; // Determins the maximum number of enemies that can be spawned
    public static EnemySpawnManager current;

    private void Awake() {
        if(current == null) {
            current = this;
            DontDestroyOnLoad(gameObject);
        } else {
            DestroyImmediate(gameObject);
            return;
        }
    }

    private void Start() {
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
    }

    private void SpawnEnemyAtSpawner() {
        if(enemies.Capacity < enemyLimit) {
            int spawnAt = Random.Range(0, spawners.Length);
            spawners[spawnAt].GetComponent<EnemySpawner>().SpawnEnemy();
        }
    }

    // Sets the number of active spawners depending on the difficulty of the current wave.
    void SetSpawners(GameObject[] spawners, int diff) {
        // A minumum of 2 spawners will always be active in the scene.
        for(int i = 0; i < Random.Range(2f * diff, spawners.Length); i++) {
            if(spawners[i].activeSelf) return;
            spawners[i].SetActive(true);
        }
    }

    // Sets all the spawners in the scene to false. Use for testing and to reset the scene after a wave.
    void DisableAllSpawners(GameObject[] spawners) {
        for(int i = 0; i < spawners.Length; i++) {
            spawners[i].SetActive(false);
        }
    }
}
