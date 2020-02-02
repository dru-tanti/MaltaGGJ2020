using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Game/Weapon")]
public class WeaponSettings : ScriptableObject {
    public Sprite weaponSprite;
    public string weaponName;
    public int fireRate;
    public int weaponDamage;
}
