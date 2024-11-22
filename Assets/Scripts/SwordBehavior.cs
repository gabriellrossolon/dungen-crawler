using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private Animator anim;
    private MeshCollider meshColl;
    private PlayerFSM playerFSM;
    public bool twoHandWeapon;

    public float swordDamage;
    public GameObject hitParticleEffect; // Prefab da part�cula a ser instanciada

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
            // Aplicar dano no inimigo
            other.GetComponent<EnemyStats>().DoDamage(swordDamage);

            // Calcular o ponto de impacto
            ContactPoint? contactPoint = CalculateContactPoint(other);

            if (contactPoint.HasValue) // Verificar se um ponto v�lido foi encontrado
            {
                // Instanciar a part�cula no ponto de impacto
                Instantiate(
                    hitParticleEffect,
                    contactPoint.Value.point,                     // Posi��o do impacto
                    Quaternion.LookRotation(contactPoint.Value.normal) // Orienta��o com base na normal
                );
            }
        }
    }

    private ContactPoint? CalculateContactPoint(Collider other)
    {
        // Usar Raycast para determinar o ponto de impacto
        Vector3 rayOrigin = transform.position - transform.forward * 3f;
        Ray ray = new Ray(rayOrigin, (other.bounds.center - rayOrigin).normalized);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1f); // O *10 � a dist�ncia do Ray, ajust�vel

        if (other.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return new ContactPoint()
            {
                point = hit.point,
                normal = hit.normal
            };
        }

        // Retornar nulo se o Raycast n�o encontrar nenhum ponto
        return null;
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(3);
        return (stateInfo.IsName("Attack1H") || stateInfo.IsName("Attack1H2") || stateInfo.IsName("Attack2H") || stateInfo.IsName("Attack2H2"))
               && (stateInfo.normalizedTime > 0.4f && stateInfo.normalizedTime < 0.5f);
    }

    // Estrutura para simular ContactPoint (se n�o for usar a nativa do Unity)
    private struct ContactPoint
    {
        public Vector3 point;
        public Vector3 normal;
    }
}
