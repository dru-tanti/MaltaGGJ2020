using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnablePool : MonoBehaviour
{
    public Rigidbody Bullet;
    public Rigidbody Shell;
    public Rigidbody Laser;
    public Rigidbody Flame;

    public int AmmountToPool;

    private List<Rigidbody> BulletList = new List<Rigidbody>();
    private List<Rigidbody> ShellList = new List<Rigidbody>();
    private List<Rigidbody> LaserList = new List<Rigidbody>();
    private List<Rigidbody> FlameList = new List<Rigidbody>();
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

            Rigidbody shell = Instantiate(Shell) as Rigidbody;
            shell.transform.parent = transform;
            ShellList.Add(shell);
            shell.gameObject.SetActive(false);

            Rigidbody laser = Instantiate(Laser) as Rigidbody;
            laser.transform.parent = transform;
            LaserList.Add(laser);
            laser.gameObject.SetActive(false);

            Rigidbody flame = Instantiate(Flame) as Rigidbody;
            flame.transform.parent = transform;
            FlameList.Add(flame);
            flame.gameObject.SetActive(false);
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

    public Rigidbody GetShell()
    {
        foreach(Rigidbody shellObj in ShellList)
        {
            if (!shellObj.gameObject.activeInHierarchy)
            {
                shellObj.gameObject.SetActive(true);
                return shellObj;
            }
        }

        Rigidbody shell = Instantiate(Shell) as Rigidbody;
        shell.transform.parent = transform;
        ShellList.Add(shell);
        return shell;
    }
    public Rigidbody GetLaser()
    {
        foreach(Rigidbody laserObj in LaserList)
        {
            if (!laserObj.gameObject.activeInHierarchy)
            {
                laserObj.gameObject.SetActive(true);
                return laserObj;
            }
        }

        Rigidbody laser = Instantiate(Laser) as Rigidbody;
        laser.transform.parent = transform;
        LaserList.Add(laser);
        return laser;
    }

    public Rigidbody GetFlame()
    {
        foreach(Rigidbody flameObj in FlameList)
        {
            if (!flameObj.gameObject.activeInHierarchy)
            {
                flameObj.gameObject.SetActive(true);
                return flameObj;
            }
        }

        Rigidbody flame = Instantiate(Flame) as Rigidbody;
        flame.transform.parent = transform;
        FlameList.Add(flame);
        return flame;
    }

}
