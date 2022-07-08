using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMouseHover : MonoBehaviour
{
    private float FontTemp;

    void Start()
    {
        FontTemp = GetComponent<TextMeshPro>().fontSize;
        GetComponent<TextMeshPro>().color = Color.white;
    }

    void OnMouseEnter()
    {
        GetComponent<TextMeshPro>().color = Color.red;
        GetComponent<TextMeshPro>().fontSize = FontTemp * 1.2f;

    }

    void OnMouseExit()
    {
        GetComponent<TextMeshPro>().color = Color.white;
        GetComponent<TextMeshPro>().fontSize = FontTemp;
    }

    private void OnMouseUp()
    {
        if (GetComponent<TextMeshPro>().text == "Quit") Application.Quit();
        if (GetComponent<TextMeshPro>().text == "Start")
        {
            GetComponent<TextMeshPro>().color = Color.cyan;
            SceneManager.LoadScene(1);
        }
    }
}
