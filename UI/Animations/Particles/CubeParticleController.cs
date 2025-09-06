using UnityEngine;
using System.Collections;

public class CubeParticleController : MonoBehaviour
{
    [Header("Particle Settings")]
    [SerializeField] private GameObject blueParticlePrefab;
    [SerializeField] private GameObject redParticlePrefab;
    [SerializeField] private GameObject greenParticlePrefab;
    [SerializeField] private GameObject yellowParticlePrefab;
    
    [Header("Animation Settings")]
    [SerializeField] private int particlesPerCube = 5;
    [SerializeField] private float particleSpeed = 3f;
    [SerializeField] private float particleLifetime = 0.8f;
    [SerializeField] private float spreadAngle = 120f;
    [SerializeField] private float gravity = -5f;
    
    private static CubeParticleController _instance;
    public static CubeParticleController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CubeParticleController>();
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        _instance = this;
    }
    
    public void PlayDestructionParticles(Vector3 position, CubeColor color)
    {
        GameObject particlePrefab = GetParticlePrefab(color);
        if (particlePrefab == null) return;
        
        for (int i = 0; i < particlesPerCube; i++)
        {
            CreateParticle(position, particlePrefab);
        }
    }
    
    private GameObject GetParticlePrefab(CubeColor color)
    {
        switch (color)
        {
            case CubeColor.b: return blueParticlePrefab;
            case CubeColor.r: return redParticlePrefab;
            case CubeColor.g: return greenParticlePrefab;
            case CubeColor.y: return yellowParticlePrefab;
            default: return null;
        }
    }
    
    private void CreateParticle(Vector3 startPosition, GameObject particlePrefab)
    {
        GameObject particle = Instantiate(particlePrefab, startPosition, Quaternion.identity);
        
        // Random trajectory
        float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
        float radians = angle * Mathf.Deg2Rad;
        
        Vector2 direction = new Vector2(
            Mathf.Sin(radians),
            Mathf.Cos(radians)
        ).normalized;
        
        // Add some randomness to speed
        float randomSpeed = particleSpeed * Random.Range(0.8f, 1.2f);
        
        // Start the particle animation
        StartCoroutine(AnimateParticle(particle, direction, randomSpeed));
    }
    
    private IEnumerator AnimateParticle(GameObject particle, Vector2 direction, float speed)
    {
        float elapsedTime = 0f;
        Vector3 velocity = direction * speed;
        
        while (elapsedTime < particleLifetime)
        {
            if (particle == null) break;
            
            // Apply gravity to velocity
            velocity.y += gravity * Time.deltaTime;
            
            // Move particle
            particle.transform.position += velocity * Time.deltaTime;
            
            // Fade out particle
            float alpha = 1f - (elapsedTime / particleLifetime);
            SpriteRenderer sr = particle.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
            
            // Optional: Add rotation
            particle.transform.Rotate(0, 0, 360f * Time.deltaTime);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (particle != null)
            Destroy(particle);
    }
}