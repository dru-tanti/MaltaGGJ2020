﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour {
    public float speed = 30f;
    public float lifetime = 3f;
    private Rigidbody _bulletRB;
    [Range(0f, 10f)]
    [Tooltip("Time from the Mortar being fired to dropping from the sky")]
    public float timeToDrop;
    public GameObject hitMarkerRing;
    public GameObject hitMarkerCircle;
    public GameObject ImpactEffect;
    public Color hitMarkerColour;
    private bool _dropping = false;
    float timer = 0f;
    private void Awake() {
        _bulletRB = GetComponent<Rigidbody>();
    }
    void Start() {
        hitMarkerCircle.GetComponent<SpriteRenderer>().color = hitMarkerColour;
        hitMarkerRing.GetComponent<SpriteRenderer>().color = hitMarkerColour;
        GameObject ring = Instantiate(hitMarkerRing, new Vector3 (transform.position.x, -0.9f ,transform.position.z),  Quaternion.Euler(90, 0, 0));
        GameObject circle = Instantiate(hitMarkerCircle, new Vector3 (transform.position.x, -0.9f ,transform.position.z),  Quaternion.Euler(90, 0, 0));
        ring.transform.parent = gameObject.transform;
        circle.transform.parent = gameObject.transform;
        StartCoroutine(DropMortar());
        Destroy(gameObject, timeToDrop + lifetime);
    }

    void Update() {
        timer += Time.deltaTime;
        transform.GetChild(3).localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(1.5f, 1.5f, 1.5f), timer / timeToDrop);
        if(_dropping) {
            _bulletRB.velocity = transform.TransformDirection(Vector3.down) * speed;
            transform.GetChild(3).position = transform.GetChild(3).TransformDirection(Vector3.up) * speed;
        }    
    }

    private IEnumerator DropMortar() {
        _dropping = false;
        yield return new WaitForSeconds(timeToDrop);
        _dropping = true;
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player has been dealt damage");
        }
        if(ImpactEffect  != null) Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
