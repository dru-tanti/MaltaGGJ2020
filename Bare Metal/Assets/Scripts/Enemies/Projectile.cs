using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 2f;
    private Rigidbody _bulletRB;
    void Awake() {
        _bulletRB = GetComponent<Rigidbody>();
    }

    void Start() {
        _bulletRB.velocity = transform.TransformDirection(Vector3.forward) * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player") {
            Debug.Log("Player Hit!");
        }
    }
}
