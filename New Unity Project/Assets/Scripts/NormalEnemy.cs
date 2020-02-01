using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : EnemyBase {
    private IEnumerator shoot;
    protected override void Awake() {
        base.Awake(); 
        fsm = GetComponent<Animator>();
        shoot = Shoot();
    }
    private void Start() {
        
        if(player) {
            fsm.SetBool("playerInRange", true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Cover") {
            fsm.SetBool("inCover", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Cover") {
            fsm.SetBool("inCover", false);
        }
    }
    public IEnumerator Shoot() {
        Instantiate(projectile, shotPoint.transform.position, transform.rotation);
        yield return new WaitForSeconds(2f);
        StartCoroutine(Shoot());
    }

    // To be used in the Animator FSM
    public void StartRoutine() {
        StartCoroutine(shoot);
    }

    public void StopRoutine() {
        StopCoroutine(shoot);
    }

}
