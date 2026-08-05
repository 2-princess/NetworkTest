using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        float h = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * h * 3f * Time.deltaTime;
    }
}
