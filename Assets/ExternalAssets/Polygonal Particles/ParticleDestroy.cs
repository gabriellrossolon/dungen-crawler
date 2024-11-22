using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        // Obt�m o componente ParticleSystem
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Verifica se a part�cula terminou de tocar
        if (_particleSystem && !_particleSystem.IsAlive())
        {
            Destroy(gameObject); // Destroi o GameObject da part�cula
        }
    }
}