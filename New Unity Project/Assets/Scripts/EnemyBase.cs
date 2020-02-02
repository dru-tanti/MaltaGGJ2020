using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    [Range(0f, 5f)]
    public float health;
    [Range(0f, 20f)]
    public float speed;
    [Range(0f, 5f)]
    public int damage;
    [Range(0f, 5f)]
    private int armour;
    public Transform shotPoint;
    public GameObject projectile;
    protected Rigidbody _rb;
    protected Animator fsm;
    protected GameObject player;

    protected virtual void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        health = health + EnemySpawnManager.current.levels[EnemySpawnManager.current.currentLevel].difficulty;
    }

    protected virtual void Update() {
        if(health <= 0) {
            EnemySpawnManager.current.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

}
