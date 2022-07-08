using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMouseHover : MonoBehaviour
{
    private float FontTemp;
    private TextMeshPro menuText;
    private TextMeshProUGUI pauseText;

    void Start()
    {
        TryGetComponent(out pauseText);
        FontTemp = TryGetComponent(out menuText) ? menuText.fontSize : pauseText.fontSize;
        if (menuText != null) menuText.color = Color.white;
    }

    void OnMouseEnter()
    {
        if (menuText != null)
        {
            menuText.fontSize = FontTemp * 1.2f;
            menuText.color = Color.red;
        }
        else pauseText.color = Color.red;
    }

    void OnMouseExit()
    {
        if (menuText != null)
        {
            menuText.color = Color.white;
            menuText.fontSize = FontTemp;
        }
        else
        {
            pauseText.color = Color.white;
            pauseText.fontSize = FontTemp;
        }
    }

    private void OnMouseUp()
    {
        if (menuText != null)
        {
            if (menuText.text == "Quit") Application.Quit();
            else if (menuText.text == "Start")
            {

                menuText.color = Color.cyan;
                SceneManager.LoadScene(1);
            }
            else if (menuText.text == "Restart") SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            if (pauseText.text == "resume") GameObject.Find("Player").GetComponent<Pause>().HandleUnPause();
            else if (pauseText.text == "quit") SceneManager.LoadScene(0);
        }
    }
}
