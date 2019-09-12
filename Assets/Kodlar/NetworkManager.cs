using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager insance;

    private void Awake()
    {
        insance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();
        ClientHandleData.InitializePackets();
        ClientTCP.InitializingNetworking();
    }

    private void OnApplicationQuit()
    {
        ClientTCP.Disconnect();
    }

}
