using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject
{
    public enum ProjectileBehavior { Bouncy, ArrowLike }

    public string projectileName;
    public GameObject projectilePrefab;
    public ProjectileBehavior behavior;
}