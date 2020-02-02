using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawner", menuName = "Game/EnemySpawner", order = 0)]
public class EnemySpawner : ScriptableObject {
    public GameObject[] enemies;
}
