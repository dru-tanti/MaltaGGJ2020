using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : Projectile {
    [Range(0f, 10f)]
    [Tooltip("Time from the Mortar being fired to dropping from the sky")]
    public float timeToDrop;
    public GameObject hitMarkerRing;
    public GameObject hitMarkerCircle;
    public GameObject ImpactEffect;
    public Color hitMarkerColour;
    private bool _dropping = false;
    float timer = 0f;
    protected override void Start() {
        hitMarkerCircle.GetComponent<SpriteRenderer>().color = hitMarkerColour;
        hitMarkerRing.GetComponent<SpriteRenderer>().color = hitMarkerColour;
        var hitmarkerring = Instantiate(hitMarkerRing, new Vector3 (transform.position.x, 0.1f ,transform.position.z),  Quaternion.Euler(90, 0, 0));
        var hitmarkercircle = Instantiate(hitMarkerCircle, new Vector3 (transform.position.x, 0.1f ,transform.position.z),  Quaternion.Euler(90, 0, 0));
        // hitmarkerring.transform.parent = gameObject.transform;
        // hitmarkercircle.transform.parent = gameObject.transform;
        StartCoroutine(DropMortar());
        Destroy(gameObject, timeToDrop + lifetime);
    }

    protected void Update() {
        timer += Time.deltaTime;
        if(!_dropping) hitMarkerCircle.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2f, 2f, 2f), timer / timeToDrop);
        if(_dropping) _bulletRB.velocity = transform.TransformDirection(Vector3.down) * speed;    
    }

    private IEnumerator DropMortar() {
        _dropping = false;
        yield return new WaitForSeconds(timeToDrop);
        _dropping = true;
    }

    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player has been dealt damage");
        }
        Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
