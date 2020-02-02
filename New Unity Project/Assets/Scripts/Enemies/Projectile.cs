using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 2f;
    protected Rigidbody _bulletRB;
    public bool Mortar;
    protected virtual void Awake() {
        _bulletRB = GetComponent<Rigidbody>();
    }

    protected virtual void Start() {
        _bulletRB.velocity = (Mortar) ? transform.TransformDirection(Vector3.up) * speed : transform.TransformDirection(Vector3.forward) * speed;
        Destroy(gameObject, lifetime);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Projectile") return;
        if (other.gameObject.tag == "PlayerHitbox") {
            Debug.Log("Player has been dealt damage");
            PlayerController.Instance.TakeDamage();
            Destroy(gameObject);
        }

    }

    private void OnBecameInvisible() {
        if(Mortar) {
            Destroy(gameObject);
        }
    }
}
