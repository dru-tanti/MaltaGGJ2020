using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/LevelSettings")]
public class LevelSettings : ScriptableObject {
    public int level;
    public int difficulty;
    public int maxEnemies;
}
