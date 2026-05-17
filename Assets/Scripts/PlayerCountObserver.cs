using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerCountObserver : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI UICounter;
    private readonly NetworkVariable<int> totalPlayerCount = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        totalPlayerCount.OnValueChanged += UpdatePlayerCount;

        UpdateCounter(totalPlayerCount.Value);

        if (!IsServer) 
            return;

        NetworkManager.Singleton.OnClientConnectedCallback += UpdateClientCount;
        NetworkManager.Singleton.OnClientDisconnectCallback -= UpdateClientCount;

        SetPlayerCount();
    }

    private void UpdateClientCount(ulong clientId)
    {
        SetPlayerCount();
    }

    private void SetPlayerCount()
    {
        totalPlayerCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
    }

    private void UpdatePlayerCount(int oldValue, int newValue)
    {  
        UpdateCounter(newValue);
    }

    private void UpdateCounter(int count)
    {
        UICounter.text = $"Players: {count}";
    }

    public override void OnNetworkDespawn()
    {
        totalPlayerCount.OnValueChanged -= UpdatePlayerCount;

        if (NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback -= UpdateClientCount;
        NetworkManager.Singleton.OnClientDisconnectCallback -= UpdateClientCount;
    }
}
