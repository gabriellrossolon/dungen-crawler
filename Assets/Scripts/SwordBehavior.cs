using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private Animator anim;
    private MeshCollider meshColl;
    private PlayerFSM playerFSM;
    public bool twoHandWeapon;

    public float swordDamage;
    public GameObject hitParticleEffect; // Prefab da partícula a ser instanciada

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

            if (contactPoint.HasValue) // Verificar se um ponto válido foi encontrado
            {
                // Instanciar a partícula no ponto de impacto
                Instantiate(
                    hitParticleEffect,
                    contactPoint.Value.point,                     // Posição do impacto
                    Quaternion.LookRotation(contactPoint.Value.normal) // Orientação com base na normal
                );
            }
        }
    }

    private ContactPoint? CalculateContactPoint(Collider other)
    {
        // Usar Raycast para determinar o ponto de impacto
        Vector3 rayOrigin = transform.position - transform.forward * 3f;
        Ray ray = new Ray(rayOrigin, (other.bounds.center - rayOrigin).normalized);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1f); // O *10 é a distância do Ray, ajustável

        if (other.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return new ContactPoint()
            {
                point = hit.point,
                normal = hit.normal
            };
        }

        // Retornar nulo se o Raycast não encontrar nenhum ponto
        return null;
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(3);
        return (stateInfo.IsName("Attack1H") || stateInfo.IsName("Attack1H2") || stateInfo.IsName("Attack2H") || stateInfo.IsName("Attack2H2"))
               && (stateInfo.normalizedTime > 0.4f && stateInfo.normalizedTime < 0.5f);
    }

    // Estrutura para simular ContactPoint (se não for usar a nativa do Unity)
    private struct ContactPoint
    {
        public Vector3 point;
        public Vector3 normal;
    }
}
