using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuMouseHover : MonoBehaviour
{
	void Start()
	{
		GetComponent<TextMeshPro>().color = Color.black;
	}

	void OnMouseEnter()
	{
		GetComponent<TextMeshPro>().color = Color.red;
		GetComponent<TextMeshPro>().fontSize = Color.red;

	}

	void OnMouseExit()
	{
		GetComponent<TextMeshPro>().color = Color.black;
	}
}
