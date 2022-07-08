using UnityEngine;
using TMPro;
using Endless.PlayerCore;

public class Pause : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pauseText;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        Instantiate(pauseText, GameObject.Find("UI Canvas").transform);
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) HandleUnPause();
    }

    private void HandleUnPause()
    {
        Cursor.visible = false;
        GetComponent<PlayerController>().enabled = true;
        Destroy(GameObject.Find("PauseText(Clone)"));
        Time.timeScale = 1;
        enabled = false;
    }
}
