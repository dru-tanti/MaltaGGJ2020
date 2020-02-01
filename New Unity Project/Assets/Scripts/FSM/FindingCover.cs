using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingCover : StateMachineBehaviour {
    private Transform playerPos;
    private GameObject targetCover; 
    public float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
       // TODO: set targetCover.isTaken = true;
        targetCover = findClosestCover(animator);
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, targetCover.transform.position, speed * Time.deltaTime); 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
       
    }

    // TODO: Check if the current cover being considered is taken.
    // Finds the closest cover to the enemy (Curtosy of Unity)
    public GameObject findClosestCover(Animator animator) {
        GameObject[] possibleCover;
        possibleCover = GameObject.FindGameObjectsWithTag("Cover");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = animator.transform.position;
        foreach (GameObject cover in possibleCover) {
            Vector3 diff = cover.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance) {
                closest = cover;
                distance = curDistance;
            }
        }
        Debug.Log("Cover Found " + closest);
        return closest;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
