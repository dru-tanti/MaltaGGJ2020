using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemy : EnemyBase {
    // Update is called once per frame
    void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.LookAt(player.transform.position); 
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player") {
            //TODO: Damage Player
            Debug.Log("Hit Player");
        }
    }
}
