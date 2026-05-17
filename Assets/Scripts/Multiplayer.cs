using System;
using UnityEngine;
using Unity.Netcode;

public class Multiplayer : MonoBehaviour
{
    [SerializeField] private GameObject selectMenu;
    [SerializeField] private bool showSelectMenu = true;
    public event Action OnUserSelected;

    private void Start()
    {
        OnUserSelected += ToggleSelectMenu;
    }
    
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        OnUserSelected?.Invoke();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        OnUserSelected?.Invoke();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        OnUserSelected?.Invoke();
    }

    private void ToggleSelectMenu()
    {
        showSelectMenu = !showSelectMenu;
        selectMenu.SetActive(showSelectMenu);
    }

    private void OnDestroy()
    {
        OnUserSelected -= ToggleSelectMenu;
    }

}
