using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject objectZone1;
    [SerializeField] GameObject objectZone2;
    [SerializeField] GameObject objectZone3;
    [SerializeField] GameObject objectZone4;
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
            string zoneTag = this.tag;
            HandleZoneActivation(zoneTag);
        }
    }

    private void HandleZoneActivation(string zoneTag)
    {
        objectZone1.SetActive(false);
        objectZone2.SetActive(false);
        objectZone3.SetActive(false);
        objectZone4.SetActive(false);

        switch (zoneTag)
        {
            case "Zone1":
                objectZone1.SetActive(true);
                break;

            case "Zone2":
                objectZone2.SetActive(true);
                break;

            case "Zone3":
                objectZone3.SetActive(true);
                break;

            case "Zone4":
                objectZone4.SetActive(true);
                break;

            default:
                Debug.LogWarning("Unknown zone tag: " + zoneTag);
                break;
        }
    }
}
