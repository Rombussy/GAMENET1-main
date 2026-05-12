using UnityEngine;
using Unity.Netcode;

public class PlayerSpawnManager : NetworkBehaviour
{
    private static int nextSpawnIndex;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) 
            return;

        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawnpoint");

        if (spawners.Length == 0)
        {
            Debug.LogWarning("No spawn points found!");
            return;
        }

        Transform availableSpawner = spawners[nextSpawnIndex].transform;
        CharacterController playerController = GetComponent<CharacterController>();

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        transform.position = availableSpawner.position;
        transform.rotation = availableSpawner.rotation;

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        nextSpawnIndex++;
        if (nextSpawnIndex >= spawners.Length)
        {
            nextSpawnIndex = 0;
        }
    }
}
