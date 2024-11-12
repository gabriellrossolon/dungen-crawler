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
        if (isAttacking)
        {
            meshColl.enabled = true;
        }
        else
        {
            meshColl.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().DoDamage(weaponDamage);
        }
    }
}
