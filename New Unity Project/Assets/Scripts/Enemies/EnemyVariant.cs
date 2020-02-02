using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariant : EnemyBase {
    [Tooltip("Firerate in rounds per minute")]
    public float fireRate = 60f;
    public float range;
    protected void Start() {
        InvokeRepeating("Shoot", 2f, (1 / (fireRate / 60)));
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.LookAt(player.transform.position); 
        if(Vector3.Distance(transform.position, player.transform.position) >= range){
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
    
    private void Shoot() {
        Instantiate(projectile, shotPoint.transform.position, transform.rotation);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, range);
    }
}
