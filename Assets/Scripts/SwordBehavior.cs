using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private Animator anim;
    private MeshCollider meshColl;

    public float swordDamage;

    private void Awake()
    {
        meshColl = GetComponent<MeshCollider>();
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }
    private void Update()
    {
        meshColl.enabled = IsAttacking(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(2);
        return (stateInfo.IsName("Attack1H") || stateInfo.IsName("Attack2H")) && stateInfo.normalizedTime > 0.3f;
    }
}
