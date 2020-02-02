using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All enemies will inherit from this class.
public class EnemyBase : MonoBehaviour {
    [Range(0f, 30f)]
    public float health;
    [Range(0f, 20f)]
    public float speed;
    public Transform shotPoint;
    public GameObject projectile;
    public GameObject pickup;
    protected GameObject player;

    protected virtual void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        // health *= EnemySpawnManager.current.levels[EnemySpawnManager.current.currentLevel].difficulty; // Scaling health;
    }

    protected virtual void Update() {
        if(health <= 0) {
            // Once this enenmy dies, remove it's reference from the EnemySpawnManager.
            EnemySpawnManager.current.enemies.Remove(gameObject);

            int chance = Random.Range(0, 5);
            if(chance == 1){
                Instantiate(pickup, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

}
