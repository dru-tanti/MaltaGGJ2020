using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    public int health;
    public int speed;
    public int damage;
    [Range(0f, 100f)]
    private int armour;
    public Transform shotPoint;
    private Rigidbody2D _rb;
    private Animator fsm;
    private GameObject player;

    private void Awake() {
        fsm = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        if(player) {
            fsm.SetBool("playerInRange", true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("We Hit " + other.name);
        if(other.tag == "Cover") {
            fsm.SetBool("inCover", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Cover") {
            fsm.SetBool("inCover", false);
        }
    }
}
