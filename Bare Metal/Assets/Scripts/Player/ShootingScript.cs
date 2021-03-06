﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ShootingScript : MonoBehaviour
{

    public enum ShootType { Bullet, Lobbing, Laser, Flame, Rocket };
    public ShootType Type;

    public enum LimbType { Arm, Shoulder };
    public LimbType Limb;
    public Transform TestBarrel;
    public GameObject BarrelEffect;
    public float _rateOfFire;
    public int _damage;
    public float _accuracy;
    public int _bullets;
    public int _projSpeed;
    public float _projLifetime;

    private float _currentRateOfFire;
    private int _currentDamage;

    public bool _autoAim;
    public bool _shotgun;

    public Transform TurretPivot;

    public LayerMask layer;
    private bool _isShooting = false;
    private float _closestEnemy = 0;
    private Transform _target;
    private SpawnablePool _spawnablePool;

    // Start is called before the first frame update
    void Awake(){
        _currentDamage = _damage;
        _currentRateOfFire = _rateOfFire;
    }
    void Start()
    {
        _spawnablePool = SpawnablePool.Instance;
        
    }

    void OnEnable(){
        Timing.RunCoroutine(ShootCoroutine().CancelWith(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if(_autoAim){
            Collider[] colliders = Physics.OverlapSphere(transform.position, 15, layer.value);

            if(colliders.Length == 0){
                _target = null;
            }

            _closestEnemy = 15;

            foreach(Collider coll in colliders){
                float dist = Vector3.Distance(transform.position, coll.transform.position);
                if(dist <= _closestEnemy){
                    _closestEnemy = dist;
                    _target = coll.transform;
                }
            }

            if(_target == null){
                _isShooting = false;
                return;
            }

            var lookPos = _target.position - TurretPivot.transform.position;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            
            TurretPivot.transform.rotation = Quaternion.Slerp(TurretPivot.transform.rotation, rotation, Time.deltaTime * 20);
        }

        if(Input.GetMouseButton(0)){
            if(Limb == LimbType.Arm){
                _isShooting = true;
            }
        }

        if(Input.GetMouseButton(1)){
            if(Limb == LimbType.Shoulder){
                _isShooting = true;
            }
        }

        if(Input.GetMouseButtonUp(0)){
            _isShooting = false;
         }

         if(Input.GetMouseButtonUp(1)){
             _isShooting = false;
         }
        
    }

    private IEnumerator<float> ShootCoroutine(){
        while(true){
            while(_isShooting){
                for(int i = -(_bullets - 1); i <= (_bullets-1); i++){
                    if(_shotgun){
                        for(int n = 0; n < 6; n++){
                            ShootProjectile(i);
                        }
                    }
                    else
                    {
                        ShootProjectile(i);
                    }

                }

                yield return Timing.WaitForSeconds(1f / _currentRateOfFire);
            }
            yield return 0f;
        }   
    }

    private void ShootProjectile(int bulletNum){

        float randY = Random.Range(-_accuracy, _accuracy);
        GameObject effect = Instantiate(BarrelEffect, TestBarrel.transform.position, Quaternion.identity) as GameObject;
        effect.transform.rotation = TestBarrel.rotation;
        effect.transform.rotation *= Quaternion.Euler(0, randY, 0);
        randY += bulletNum * 12;

        switch (Type){

            case ShootType.Bullet:
                Rigidbody proj = _spawnablePool.GetBullet();
                proj.GetComponent<BulletScript>().SetStats(_currentDamage ,_projLifetime);

                proj.transform.position = TestBarrel.position;
                proj.transform.rotation = TestBarrel.rotation;
                proj.transform.rotation *= Quaternion.Euler(90, randY, 0);
                proj.gameObject.GetComponent<TrailRenderer>().Clear();
                proj.AddForce(proj.transform.up * _projSpeed, ForceMode.VelocityChange);
                break;
                
            case ShootType.Lobbing:
                float dist = Vector3.Distance(transform.position, PlayerController.Instance.LookPoint);
                Rigidbody shell = _spawnablePool.GetShell();
                shell.GetComponent<BulletScript>().SetStats(_currentDamage, _projLifetime);

                if(dist < 3){
                    dist = 3;
                }

                if(dist > 16){
                    dist = 16;
                }
                shell.transform.position = TestBarrel.position;
                shell.transform.rotation = TestBarrel.rotation;
                shell.transform.rotation *= Quaternion.Euler(90, randY, 0);
                shell.gameObject.GetComponent<TrailRenderer>().Clear();
                shell.AddForce(shell.transform.up * (dist * Random.Range(2.1f, 3.3f)), ForceMode.VelocityChange);
                break;
            case ShootType.Laser:
                Rigidbody laserProj = _spawnablePool.GetLaser();
                laserProj.GetComponent<BulletScript>().SetStats(_currentDamage, _projLifetime);

                laserProj.transform.position = TestBarrel.position;
                laserProj.transform.rotation = TestBarrel.rotation;
                laserProj.transform.rotation *= Quaternion.Euler(90, randY, 0);
                laserProj.gameObject.GetComponent<TrailRenderer>().Clear();
                laserProj.AddForce(laserProj.transform.up * _projSpeed, ForceMode.VelocityChange);
                break;
            case ShootType.Flame:
                Rigidbody flameProj = _spawnablePool.GetFlame();
                flameProj.GetComponent<BulletScript>().SetStats(_currentDamage, _projLifetime);

                flameProj.transform.position = TestBarrel.position;
                flameProj.transform.rotation = TestBarrel.rotation;
                flameProj.transform.rotation *= Quaternion.Euler(90, randY, 0);
                flameProj.gameObject.GetComponent<TrailRenderer>().Clear();
                flameProj.AddForce(flameProj.transform.up * _projSpeed, ForceMode.VelocityChange);
                break;
            case ShootType.Rocket:
                Rigidbody rocketProj = _spawnablePool.GetRocket();
                rocketProj.GetComponent<BulletScript>().SetStats(_currentDamage, _projLifetime);

                rocketProj.transform.position = TestBarrel.position;
                rocketProj.transform.rotation = TestBarrel.rotation;
                rocketProj.transform.rotation *= Quaternion.Euler(90, randY, 0);
                rocketProj.gameObject.GetComponent<TrailRenderer>().Clear();
                rocketProj.AddForce(rocketProj.transform.up * _projSpeed, ForceMode.VelocityChange);
                break;
        }
    }

    public void ScaleStats(){
        _currentRateOfFire = _rateOfFire * EnemySpawnManager.Instance.currentLevel + 1;
        _currentDamage = _damage * EnemySpawnManager.Instance.currentLevel + 1;
    }
}
