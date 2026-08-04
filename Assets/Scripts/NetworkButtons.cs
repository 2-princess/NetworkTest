using Unity.Netcode;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300 , 200), "Host 시작"))
        {
            NetworkManager.Singleton.StartHost();
        }

        if (GUI.Button(new Rect(170, 10, 300, 200), "Client 접속"))
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}