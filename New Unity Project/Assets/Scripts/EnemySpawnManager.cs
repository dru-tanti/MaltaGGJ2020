using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

    [Header("Spawning Variables")]
    [Range(0, 20)]
    public int enemyLimit; // Determins the maximum number of enemies that can be spawned
    [Range(0f, 10f)]
    public float spawnRate;
    [Tooltip("Set the rate of enemy spawns in seconds")]
    public int currentLevel = 0;

    private int enemiesSpawned;

    [Header("Reference Variables")]
    public LevelSettings[] levels;
    public GameObject[] spawners; // Reference for all the spawners in the scene.
    public List<GameObject> enemies = new List<GameObject>(); // Keeps a reference of all the spawned enemies currently in the scene.
    public static EnemySpawnManager current;
    private bool sceneEmpty;
    private IEnumerator spawnEnemy;
    private void Awake() {
        enemyLimit = levels[currentLevel].maxEnemies;
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
        SetSpawners(spawners, 1);
        spawnEnemy = SpawnEnemyAtSpawner();
    }

    private void Update() {
        if(Input.GetKeyDown("space")) {
            Debug.Log("Stopping");
            StopCoroutine(spawnEnemy);
        }
        if(enemiesSpawned >= this.levels[currentLevel].maxEnemies) {
            StopCoroutine(spawnEnemy);
        }

        if(enemies.Count == 0) {
            currentLevel++;
            enemiesSpawned = 0;
            StartCoroutine(spawnEnemy);
        }
    }

    private IEnumerator SpawnEnemyAtSpawner() {
        if(enemies.Count < levels[currentLevel].maxEnemies) {
            enemiesSpawned++;
            int spawnAt = Random.Range(0, spawners.Length);
            spawners[spawnAt].GetComponent<SpawnPoint>().SpawnEnemy();
        }

        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnEnemyAtSpawner());
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
