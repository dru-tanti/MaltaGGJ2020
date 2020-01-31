﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnablePool : MonoBehaviour
{
    public Rigidbody Bullet;

    public int AmmountToPool;

    private List<Rigidbody> BulletList = new List<Rigidbody>();
        private List<GameObject> DamageTextList = new List<GameObject>();

    private static SpawnablePool _instance;

    public static SpawnablePool Instance { get { return _instance; } }

    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        for(int i = 0; i < AmmountToPool; i++) {
            Rigidbody bullet = Instantiate(Bullet) as Rigidbody;
            bullet.transform.parent = transform;
            BulletList.Add(bullet);
            bullet.gameObject.SetActive(false);
        }
            
    }

    public Rigidbody GetBullet()
    {
        foreach(Rigidbody bulletObj in BulletList)
        {
            if (!bulletObj.gameObject.activeInHierarchy)
            {
                bulletObj.gameObject.SetActive(true);
                return bulletObj;
            }
        }

        Rigidbody bullet = Instantiate(Bullet) as Rigidbody;
        bullet.transform.parent = transform;
        BulletList.Add(bullet);
        return bullet;
    }

}
