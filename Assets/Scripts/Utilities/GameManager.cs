using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject zone1;
    [SerializeField] GameObject zone2;
    [SerializeField] GameObject zone3;
    [SerializeField] GameObject zone4;
    private float deltaTime = 0.0f;

    void Start()
    {
        Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int width = Screen.width, height = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(10, 10, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 100;
        style.normal.textColor = Color.white;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS", fps);
        GUI.Label(rect, text, style);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string zoneTag = gameObject.tag;
            HandleZoneActivation(zoneTag);
        }
    }

    private void HandleZoneActivation(string zoneTag)
    {
        switch (zoneTag)
        {
            case "zone1":
                zone1.SetActive(true);
                break;

            case "zone2":
                zone2.SetActive(true);
                break;

            case "zone3":
                zone3.SetActive(true);
                break;

            case "zone4":
                zone4.SetActive(true);
                break;

            default:
                Debug.LogWarning("Unknown zone tag: " + zoneTag);
                break;
        }
    }
}
