using UnityEngine;

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
            Destroy(this.gameObject);
        }
    }
}
