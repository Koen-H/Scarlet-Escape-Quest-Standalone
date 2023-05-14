using _Scripts.Minigames;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NerworkProtocolManager : NetworkBehaviour
{
    private static NerworkProtocolManager _instance;
    public static NerworkProtocolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("NerworkProtocolManager is null");
            }

            return _instance;
        }
    }



    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestClueServerRpc()
    {
        MinigameFSM.Instance.GetClue();
    }
        

    [ClientRpc]
    public void ClueNotReadyClientRpc()
    {
        CanvasManager.Instance.PhoneConsoleMessage("Clue not ready");
        //Do stuff on clien
    }

    [ClientRpc]
    public void SetClueCooldownClientRpc(float time)
    {
        CanvasManager.Instance.SetClueCountdown(time);
    }


    [ClientRpc]
    public void WinGameClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        CanvasManager.Instance.GetComponent<MobileCanvasFSM>().LoadState(MobileState.WON);
    }

    [ClientRpc]
    public void LoseGameClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        CanvasManager.Instance.GetComponent<MobileCanvasFSM>().LoadState(MobileState.LOST);
    }

    [ServerRpc]
    public void SendSpeechServerRpc()
    {
        //Send speech to server
    }

}
