using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    public float maxHp;
    public float actualHp;

    private void Start()
    {
        actualHp = maxHp;
    }

    public void DoDamage(float damage)
    {
        actualHp -= damage;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if(actualHp <= 0)
        {
            GetComponentInChildren<Animator>().SetTrigger("isDie");
            GetComponent<EnemyFSM>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            Destroy(this.gameObject, 4f);
        }
    }
}
