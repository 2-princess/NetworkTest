using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (playerHealth.IsDead()) return;
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += new Vector3(h, 0, v) * 5 * Time.deltaTime;

    }
}
