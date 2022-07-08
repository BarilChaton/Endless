using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuMouseHover : MonoBehaviour
{
	private float FontTemp;

	void Start()
	{
		FontTemp = GetComponent<TextMeshPro>().fontSize;
		GetComponent<TextMeshPro>().color = Color.black;
	}

	void OnMouseEnter()
	{
		GetComponent<TextMeshPro>().color = Color.red;
		GetComponent<TextMeshPro>().fontSize = FontTemp * 1.2f;

	}

	void OnMouseExit()
	{
		GetComponent<TextMeshPro>().color = Color.black;
		GetComponent<TextMeshPro>().fontSize = FontTemp;
	}
}
