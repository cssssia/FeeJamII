using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public List<Sprite> sprites;
    public List<AnimatorController> animators;
    public List<Vector2> spawnPositions;
}
