﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class AbilityScript : MonoBehaviour
{
    public enum AbilityType { Shield, Dash, Shockwave};
    public AbilityType Type;
    public float _internalWaitTime;
    public KeyCode KeyRequired;
    private bool _canActivate = true;
    public GameObject Effect;
    public LayerMask layer;

    private PlayerController playerController;


    void Update(){
        if(playerController == null){
            playerController = PlayerController.Instance;
            playerController.AddAbility(this);
        }
    }
    void OnEnable(){
        if(playerController != null){
            playerController.AddAbility(this);
        }

    }

    void OnDisable(){
        if(playerController != null){
            playerController.RemoveAbility(this);
        }
    }
    private IEnumerator<float> InternalTimer(){
        yield return Timing.WaitForSeconds(_internalWaitTime);

        _canActivate = true;
    }

    public void ActivateAbility(KeyCode keyPressed){
        if(_canActivate){
            if(keyPressed == KeyRequired){
                Debug.Log("Activated Ability");
                switch (Type){
                    case AbilityType.Shockwave:
                        Instantiate(Effect, transform.position, Quaternion.identity);
                        Collider[] colliders = Physics.OverlapSphere(transform.position, 15, layer.value);

                        if(colliders.Length != 0){
                            foreach(Collider coll in colliders){
                                Vector3 dir = coll.transform.position - transform.position;
                                dir = dir.normalized;

                                coll.gameObject.GetComponent<Rigidbody>().AddForce(dir * 20, ForceMode.VelocityChange);
                            }
                        }

                        break;
                    case AbilityType.Dash:
                        transform.root.transform.GetComponent<Rigidbody>().AddRelativeForce(PlayerController.Instance.PlayerLegs.forward * 400);

                        break;
                }
                _canActivate = false;
                Timing.RunCoroutine(InternalTimer().CancelWith(gameObject));
            }
        }
    }
}
