using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkPileMenu : KaijuCallMenu<JunkPileMenu> {

	public void OnPressedPile()
	{
		throw new Exception("Junk Pile not implemented yet");
	}

	public void Show(JunkPile junkPile)
	{
		Open();
	}
}
