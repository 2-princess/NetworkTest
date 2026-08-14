using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 200), "Host 시작"))
        {
            ConnectManager.Instance.StartHost();
        }
    }
}