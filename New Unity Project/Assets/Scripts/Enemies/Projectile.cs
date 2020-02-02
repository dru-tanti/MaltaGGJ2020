using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 2f;
    protected Rigidbody _bulletRB;
    protected virtual void Awake() {
        _bulletRB = GetComponent<Rigidbody>();
    }

    protected virtual void Start() {
        _bulletRB.velocity = transform.TransformDirection(Vector3.forward) * speed;
        Destroy(gameObject, lifetime);
    }
}
