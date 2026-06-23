using UnityEngine;
using Unity.Netcode;

public class NetworkProjectile : NetworkBehaviour
{
    [SerializeField] private float speed = 12.5f;
    [SerializeField] private float lifeTime = 10.0f; // Fixed: Removed double 'ff' typo
    private float despawnTime;

    public override void OnNetworkSpawn()
    {
        // Only the server should manage the lifetime clock
        if (IsServer)
        {
            despawnTime = Time.time + lifeTime;
        }
    }

    void Update()
    {
        // Only the server moves the object and calculates despawning
        if (!IsServer) return;

        // Moves the projectile forward smoothly
        transform.position += transform.forward * speed * Time.deltaTime;

        // Despawns the projectile if it exceeds its lifetime
        if (Time.time >= despawnTime)
        {
            NetworkObject.Despawn(); // Fixed: Despawn the instance's NetworkObject
        }
    }

    // Fixed: Corrected the parameter syntax (Collider other)
    private void OnTriggerEnter(Collider other) 
    {
        // Only the server handles hit detection and despawning
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            NetworkObject.Despawn();
        }
    }
}