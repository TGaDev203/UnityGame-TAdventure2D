using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private PolygonCollider2D zone1;
    [SerializeField] private PolygonCollider2D zone2;
    [SerializeField] private PolygonCollider2D zone3;
    [SerializeField] private PolygonCollider2D zone4;

    private CinemachineConfiner2D confiner;

    private void Start()
    {
        if (virtualCamera != null)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
            if (confiner == null)
            {
                Debug.LogError("CinemachineConfiner2D not found on the virtual camera!");
            }
        }
        else
        {
            Debug.LogError("Virtual Camera is not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ZoneControl();
        }
    }

    private void ZoneControl()
    {
        switch (tag)
        {
            case "Zone1":
                confiner.m_BoundingShape2D = zone1;
                break;

            case "Zone2":
                confiner.m_BoundingShape2D = zone2;
                break;

            case "Zone3":
                confiner.m_BoundingShape2D = zone3;
                break;

            case "Zone4":
                confiner.m_BoundingShape2D = zone4;
                break;

            default:
                Debug.LogWarning("No valid zone tag matched.");
                break;
        }
    }
}
