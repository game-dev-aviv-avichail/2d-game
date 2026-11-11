
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float cameraZ;

    void Start()
    {
        cameraZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x,
            player.position.y,
            cameraZ
        );
    }
}
