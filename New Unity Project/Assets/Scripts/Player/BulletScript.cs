﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using MEC;
using TMPro;

public class BulletScript : MonoBehaviour
{

    public enum ProjectileType { Bullet, Explosive };
    public ProjectileType Type;
    public GameObject ImpactEffect;
    private const string ENEMY_TAG = "Enemy";
    private float _dmg;
    private bool lob = false;
    private SpawnablePool _spawnPool;

    private Vector3 _targetPos = Vector3.zero;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        if(_spawnPool == null)
        {
            _spawnPool = SpawnablePool.Instance;
        }
        Timing.RunCoroutine(StartDespawn().CancelWith(gameObject));
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == ENEMY_TAG)
        {

        }

        if(ImpactEffect != null){
            Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        }


        Despawn();
    }

    void FixedUpdate()
    {
        _rb.AddForce(Physics.gravity * _rb.mass, ForceMode.Acceleration);
        if(_rb.velocity != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(_rb.velocity);
            transform.rotation *= Quaternion.Euler(90,0,0);
        }

    }

    IEnumerator<float> StartDespawn()
    {
        yield return Timing.WaitForSeconds(5);
        //Despawn();
    }

    private void Despawn()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
    public void SetStats(float dmg)
    {
        _dmg = dmg;
    }
}
