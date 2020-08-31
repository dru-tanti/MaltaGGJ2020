using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    public static EnemySpawnManager current;

    [Header("Spawning Variables")]
    [Range(0, 20)]
    public int enemyLimit; // Determins the maximum number of enemies that can be spawned
    [Tooltip("Set the rate of enemy spawns in seconds")]
    [Range(0f, 10f)]
    public float spawnRate; // Rate the enenemies spawn in seconds
    [Header("Level Settings")]
    [HideInInspector] public int currentLevel = 0;
    public LevelSettings[] levels;

    [Header("Reference Variables")]
    [HideInInspector] public GameObject[] spawners; // Reference for all the spawners in the scene.
    [HideInInspector] public List<GameObject> enemies = new List<GameObject>(); // Keeps a reference of all the spawned enemies currently in the scene.
    private int enemiesSpawned;
    private bool spawnEnemy; // To determine whether enemies should be spawned in the scene.

    private static EnemySpawnManager _instance;
    public static EnemySpawnManager Instance {get{return _instance;}}
    private void Awake() {
        _instance = this;
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
        spawnEnemy = true;
        Debug.Log(currentLevel+ " : " + levels[currentLevel].maxEnemies);
        StartCoroutine(SpawnEnemyAtSpawner());
    }

    private void Update() {
        // If the total number of enemies spawned is greater than what we set in the maximum, then stop the spawning.
        if(enemiesSpawned >= this.levels[currentLevel].maxEnemies) {
            spawnEnemy = false;
        }

        if(enemies.Count == 0) {
            AdvanceLevel();
        }
    }

    // Chooses a random spawner and spawns an enemy at the rate we provide.
    private IEnumerator SpawnEnemyAtSpawner() {
        while(spawnEnemy == true) {
            if(enemies.Count < levels[currentLevel].maxEnemies) {
                enemiesSpawned++;
                int spawnAt = Random.Range(0, spawners.Length);
                spawners[spawnAt].GetComponent<SpawnPoint>().SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnRate);
            StartCoroutine(SpawnEnemyAtSpawner());
        }
    }

    // Advance the level, and reset the spawning data.
    void AdvanceLevel() {
        currentLevel++;
        enemiesSpawned = 0;
        spawnEnemy = true;
        DisableAllSpawners(spawners);
        SetSpawners(spawners, levels[currentLevel].difficulty);
        StartCoroutine(SpawnEnemyAtSpawner());
        Debug.Log(currentLevel + " : " + levels[currentLevel].maxEnemies);
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
