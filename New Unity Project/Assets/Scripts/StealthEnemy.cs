using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : EnemyBase {
    public float range;
    public float lungeMultiplier;
    public float waitTime;
    public float moveTime;    
    private float time;

    private void Start() {
        StartCoroutine(stalkPlayer());
    }

    private IEnumerator stalkPlayer() {
        bool stalk  = true;
        time = waitTime;
        while(stalk && time <= 0) {
            // If the player is within the attack range, lunge at the player.
            speed = (Vector3.Distance(transform.position, player.transform.position) > range) ? speed * lungeMultiplier : speed;
            transform.LookAt(player.transform.position);
            _rb.velocity = new Vector3 (0f, 0f, speed * Time.deltaTime);
            time -= Time.deltaTime;
        }
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(stalkPlayer());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, range);
    }
}
