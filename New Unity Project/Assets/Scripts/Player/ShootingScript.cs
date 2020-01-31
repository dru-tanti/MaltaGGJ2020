using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ShootingScript : MonoBehaviour
{

    public Rigidbody TestBullet;
    public Transform TestBarrel;

    private float _rateOfFire = 5;
    private int _damage = 25;
    private float _accuracy = 5f;
    private int _bullets = 1;

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
        Rigidbody proj = _spawnablePool.GetBullet();
        proj.GetComponent<BulletScript>().SetStats(_damage);
        proj.transform.position = TestBarrel.position;
        proj.transform.rotation = TestBarrel.rotation;
        proj.transform.rotation *= Quaternion.Euler(90, randY, 0);
        proj.AddForce(proj.transform.up * 50f, ForceMode.VelocityChange);
    }
}
