using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ExpandPreferredHeight : MonoBehaviour {
	
	// The rect component of this
	private RectTransform rectTransform;
	
	// The layout element of this
	private ILayoutElement layoutElement;
	
	// Paddings
	public int paddingBottom;

	// Use this for initialization
	void Start ()
	{
		rectTransform = gameObject.GetComponent<RectTransform>();
		layoutElement = (ILayoutElement) gameObject.GetComponent(typeof(ILayoutElement));
	}
	
	// Update is called once per frame
	void Update () {
		try
		{
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, layoutElement.preferredHeight + paddingBottom);
		}
		catch (Exception e)
		{
		}
	}
}
