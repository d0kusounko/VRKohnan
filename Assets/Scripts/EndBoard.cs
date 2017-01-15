using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndBoard : MonoBehaviour
{
	private Text endText;

	// Use this for initialization
	void Start()
	{
		endText = transform.FindChild( "Text" ).gameObject.GetComponent<Text>();
		endText.enabled = false;
	}

	public void SetVisible( bool visible )
	{
		endText.enabled = visible;
	}
}
