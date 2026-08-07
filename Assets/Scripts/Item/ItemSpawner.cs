using Unity.Netcode;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject itemPrefeb;

    public void SpawnItem()
    {
        if (!IsServer) return;

        GameObject item = Instantiate(itemPrefeb);
        item.GetComponent<NetworkObject>().Spawn();
    }

    void Update()
    {
        if(!IsServer) return;
        if (Input.GetKeyDown(KeyCode.I))
        {
            SpawnItem();
        }
    }
}
