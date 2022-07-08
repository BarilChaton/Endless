using UnityEngine;
using TMPro;
using Endless.PlayerCore;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        Instantiate(pauseScreen, GameObject.Find("UI Canvas").transform);
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) HandleUnPause();
    }

    public void HandleUnPause()
    {
        Cursor.visible = false;
        GetComponent<PlayerController>().enabled = true;
        Destroy(GameObject.Find("PauseScreen(Clone)"));
        Time.timeScale = 1;
        enabled = false;
    }
}
