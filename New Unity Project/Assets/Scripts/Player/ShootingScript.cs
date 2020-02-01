using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ShootingScript : MonoBehaviour
{

    public enum ShootType { Bullet, Lobbing };
    public ShootType Type;
    public Rigidbody TestBullet;
    public Transform TestBarrel;

    public float _rateOfFire;
    public int _damage;
    public float _accuracy;
    public int _bullets;

    private bool _isShooting = false;

    private SpawnablePool _spawnablePool;

    // Start is called before the first frame update
    void Start()
    {
        _spawnablePool = SpawnablePool.Instance;
        Timing.RunCoroutine(ShootCoroutine().CancelWith(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            _isShooting = true;
        }

        if(Input.GetMouseButtonUp(0)){
            _isShooting = false;
        }
    }

    private IEnumerator<float> ShootCoroutine(){
        while(true){
            while(_isShooting){
                for(int i = -(_bullets - 1); i <= (_bullets-1); i++){
                    ShootProjectile(i);
                }

                yield return Timing.WaitForSeconds(1f / _rateOfFire);
            }
            yield return 0f;
        }   
    }

    private void ShootProjectile(int bulletNum){
        float randY = Random.Range(-_accuracy, _accuracy);
        randY += bulletNum * 12;

        switch (Type){

            case ShootType.Bullet:
                Rigidbody proj = _spawnablePool.GetBullet();
                proj.GetComponent<BulletScript>().SetStats(_damage);
                proj.transform.position = TestBarrel.position;
                proj.transform.rotation = TestBarrel.rotation;
                proj.transform.rotation *= Quaternion.Euler(90, randY, 0);
                proj.AddForce(proj.transform.up * 125f, ForceMode.VelocityChange);
                break;
            case ShootType.Lobbing:
                float dist = Vector3.Distance(transform.position, PlayerController.Instance.LookPoint);
                Rigidbody shell = _spawnablePool.GetShell();
                shell.GetComponent<BulletScript>().SetStats(_damage);
                if(dist < 3){
                    dist = 3;
                }

                if(dist > 16){
                    dist = 16;
                }
                shell.transform.position = TestBarrel.position;
                shell.transform.rotation = TestBarrel.rotation;
                shell.transform.rotation *= Quaternion.Euler(90, randY, 0);
                shell.AddForce(shell.transform.up * (dist * Random.Range(2.1f, 3.3f)), ForceMode.VelocityChange);
                break;
        }

    }
}
