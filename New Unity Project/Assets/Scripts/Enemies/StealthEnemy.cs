using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealthEnemy : EnemyBase {
    public float range;
    public float lungeMultiplier;
    public float waitTime;

    private bool evaluating;
    private bool playerInRange = false;
    private void Start() {
        StartCoroutine(huntPlayer());
    }

    protected override void Update() {
        base.Update();
        if(!playerInRange && !evaluating) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.LookAt(player.transform.position);
        }

        if(playerInRange && !evaluating) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (speed * lungeMultiplier) * Time.deltaTime);
            transform.LookAt(player.transform.position);
        }
    }

    private IEnumerator huntPlayer() {
        evaluating = true;
        playerInRange = (Vector3.Distance(transform.position, player.transform.position) < range);
        yield return new WaitForSeconds(waitTime);
        evaluating = false;
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(huntPlayer());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, range);
    }
}
