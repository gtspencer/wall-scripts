using UnityEngine;

public class OutwardParticleVelocity : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps == null) return;

        int maxParticles = ps.main.maxParticles;
        if (particles == null || particles.Length < maxParticles)
        {
            particles = new ParticleSystem.Particle[maxParticles];
        }

        int particleCount = ps.GetParticles(particles);
        Vector3 center = transform.position;

        for (int i = 0; i < particleCount; i++)
        {
            Vector3 direction = (particles[i].position - center).normalized;
            particles[i].velocity = direction * 5f; // Adjust speed as needed
        }

        ps.SetParticles(particles, particleCount);
    }
}