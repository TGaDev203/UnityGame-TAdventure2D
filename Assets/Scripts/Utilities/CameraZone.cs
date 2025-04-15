using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class CameraZone : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private List<GameObject> backgrounds;
    [SerializeField] private List<PolygonCollider2D> zones;

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
        if (!tag.StartsWith("Zone") || !int.TryParse(tag.Substring(4), out int zoneNumber))
        {
            Debug.LogWarning($"Invalid zone tag format: {tag}");
            return;
        }

        int index = zoneNumber - 1;

        if (index < 0 || index >= zones.Count)
        {
            Debug.LogWarning($"Zone index {index} is out of range.");
            return;
        }

        confiner.m_BoundingShape2D = zones[index];

        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].SetActive(i == index);
        }
    }

}
