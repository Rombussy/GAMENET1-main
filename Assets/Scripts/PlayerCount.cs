using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
public class PlayerCount : MonoBehaviour
{
public TMP_Text playerCountText;
    void Update()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            int playerCount = NetworkManager.Singleton.ConnectedClients.Count;
            playerCountText.text = "Players: " + playerCount;
        }
    }
}
