using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public List<Sprite> sprites;
    public List<RuntimeAnimatorController> animators;
    public List<Vector2> spawnPositions;
}