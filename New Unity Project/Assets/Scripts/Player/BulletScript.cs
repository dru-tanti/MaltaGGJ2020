using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using MEC;
using TMPro;

public class BulletScript : MonoBehaviour
{

    public enum ProjectileType { Bullet, Grenade , Rocket};
    public ProjectileType Type;
    public GameObject ImpactEffect;
    public LayerMask layer;
    private const string ENEMY_TAG = "Enemy";
    private float _dmg;
    private float _lifeTime;
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
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == ENEMY_TAG)
        {
            EnemyBase enemy = col.gameObject.GetComponent<EnemyBase>();
            enemy.health -= _dmg;
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

        if(Type == ProjectileType.Rocket){
            Debug.Log("Rocket");
            _rb.AddForce((PlayerController.Instance.LookPoint - transform.position) * 4, ForceMode.Acceleration);
        }
        _lifeTime -= Time.deltaTime;

        if(_lifeTime <= 0){
            Despawn();
        }

    }

    private void Despawn()
    {
        if(ImpactEffect != null){
            Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        }

        if(Type == ProjectileType.Grenade || Type == ProjectileType.Rocket){
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2, layer.value);

            foreach(Collider coll in colliders){
                EnemyBase enemy = coll.gameObject.GetComponent<EnemyBase>();
                enemy.health -= _dmg;
            }
            float stress = 2 / Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
            stress = Mathf.Clamp(stress, 0, 0.4f);
            Camera.main.GetComponent<StressReceiver>().InduceStress(stress);
        }

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
    public void SetStats(float dmg, float lifeTime)
    {
        _lifeTime = lifeTime;
        _dmg = dmg;
    }
}
