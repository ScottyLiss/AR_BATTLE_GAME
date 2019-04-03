using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodsMenu : KaijuCallMenu<FoodsMenu> {

	public static void Show(GameObject parentGameObject)
	{
		Open();
		
		Instance.transform.SetParent(parentGameObject.transform);
	}
}
