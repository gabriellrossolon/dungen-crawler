using UnityEngine;

public class EnemyWeaponBehavior : MonoBehaviour
{
    public float weaponDamage;
    private MeshCollider meshColl;
    public bool isAttacking;
    void Awake()
    {
        meshColl = GetComponent<MeshCollider>();
    }

    void Update()
    {
        meshColl.enabled = isAttacking;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            other.GetComponent<PlayerStats>().DoDamage(weaponDamage);
        }
    }
}
