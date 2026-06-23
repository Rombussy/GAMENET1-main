using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerShooter : NetworkBehaviour
{
    // variables
    [SerializeField] private GameObject bulletPrefab; // bullet prefab to instantiate when shooting
    [SerializeField] private Transform bulletSpawnPoint; // point from which the bullet will be spawned
    [SerializeField] private float fireCooldown = 0.5f; // time in seconds between shots
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0; // Fixed: KeyCode capitalized
    private float lastFireTime;

    void Update()
    {
        if (!IsOwner) return; // only allow the local player to shoot

        // Fixed: fireKey case, and changed cooldown logic to properly check if enough time has passed
        if (Input.GetKeyDown(fireKey) && Time.time >= lastFireTime)
        {
            lastFireTime = Time.time + fireCooldown;
            // Fixed: bulletSpawnPoint case and added missing semicolon
            RequestShootServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.forward); 
        }
    }

    [ServerRpc]
    private void RequestShootServerRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        // Fixed: Typo in projectileInstance variable name
        GameObject projectileInstance = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.LookRotation(spawnDirection)
        );

        // Tells Unity Netcode to spawn this object across the network
        NetworkObject networkObject = projectileInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn();
        }
    }
}