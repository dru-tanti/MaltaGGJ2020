using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarEnemy : EnemyBase
{
    [Tooltip("Firerate in rounds per minute")]
    public float fireRate = 60f;
    public float range = 15f;
    public GameObject mortar;
    protected void Start() {
        InvokeRepeating("FireMortar", 2f, (1 / (fireRate / 60)));
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.LookAt(player.transform.position); 
        if(Vector3.Distance(transform.position, player.transform.position) >= range / 2){
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        if(Vector3.Distance(transform.position, player.transform.position) >= range ) {
            // FireMortar();
        } 
    }
    
    private void FireMortar() {
        Instantiate(projectile, shotPoint.transform.position, Quaternion.identity);
        Instantiate(mortar, new Vector3 (player.transform.position.x, 25f, player.transform.position.z), Quaternion.identity);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, range);
    }
}
