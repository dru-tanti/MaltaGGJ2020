using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 2f;
    private Rigidbody _bulletRB;

    private void Awake() {
        _bulletRB = GetComponent<Rigidbody>();
    }

    private void Start() {
        _bulletRB.velocity = transform.TransformDirection(Vector3.forward) * speed;
        // Destroy this object after a specific amount of time.
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player has been dealt damage");
        }

        // The laser should be destroyed if it hits anything.
        Destroy(gameObject);
    }
}
