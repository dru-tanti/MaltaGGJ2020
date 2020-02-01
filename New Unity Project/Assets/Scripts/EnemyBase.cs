using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    [Range(0f, 5f)]
    public int health;
    [Range(0f, 5f)]
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
    }

}
