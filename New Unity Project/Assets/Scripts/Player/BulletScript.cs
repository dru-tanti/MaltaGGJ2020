using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using MEC;
using TMPro;

public class BulletScript : MonoBehaviour
{
    private const string ENEMY_TAG = "Enemy";
    private float _dmg;
    private SpawnablePool _spawnPool;

    // Start is called before the first frame update
    void OnEnable()
    {
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
        Despawn();
    }

    IEnumerator<float> StartDespawn()
    {
        yield return Timing.WaitForSeconds(5);
        Despawn();
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
