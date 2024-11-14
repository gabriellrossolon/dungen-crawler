using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private Animator anim;
    private MeshCollider meshColl;
    private PlayerFSM playerFSM;
    public bool twoHandWeapon;

    public float swordDamage;

    private void Awake()
    {
        meshColl = GetComponent<MeshCollider>();
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
        playerFSM = GetComponentInParent<PlayerFSM>();
    }
    private void Update()
    {
        meshColl.enabled = IsAttacking();

        playerFSM._usingTwoHand = twoHandWeapon;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStats>().DoDamage(swordDamage);
        }
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(3);
        return (stateInfo.IsName("Attack1H") || stateInfo.IsName("Attack1H2") || stateInfo.IsName("Attack2H") || stateInfo.IsName("Attack2H2")) && stateInfo.normalizedTime > 0.4f;
    }
}
