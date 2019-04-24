using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public MainScreen MainScreenPrefab;
	public BreachesMenu BreachesMenuPrefab;
	public CatalystsMenu CatalystsMenuPrefab;
	public TraitsMenu TraitsMenuPrefab;
	public JunkPileMenu JunkPileMenuPrefab;
	public PetMenu PetMenuPrefab;
	public FoodsMenu FoodsMenuPrefab;
	public BreachViewMenu BreachViewMenuPrefab;
    public InfoMenu InfoMenuMain;
    public InfoPetMenu InfoMenuPet;
    public InfoCataMenu InfoMenuCatalyst;
    public InfoTraitMenu InfoMenuTraits;
    public ConfirmRemovalBeacon RemoveBeaconMennu;

    public HealthWarningPopup HealthWarningPopupPrefab;

    private Stack<KaijuCallMenu> menuStack = new Stack<KaijuCallMenu>();

    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        StaticVariables.menu = this;
        Instance = this;

        MainScreen.Show();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

	public void CreateInstance<T>() where T : KaijuCallMenu
	{
		var prefab = GetPrefab<T>();

		Instantiate(prefab, transform);
	}

	public void OpenMenu(KaijuCallMenu instance)
    {
        // De-activate top menu
        if (menuStack.Count > 0)
        {
			if (instance.DisableMenusUnderneath && instance.tag != "Info")
			{
				foreach (var menu in menuStack)
				{
					menu.gameObject.SetActive(false);

					if (menu.DisableMenusUnderneath)
						break;
				}
			}

            if(instance.tag != "Info")
            {
                var topCanvas = instance.GetComponent<Canvas>();
                var previousCanvas = menuStack.Peek().GetComponent<Canvas>();
                topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
            }
        }
		
        if (instance.AddToStack && instance.tag != "Info")
			menuStack.Push(instance);
    }

    private T GetPrefab<T>() where T : KaijuCallMenu
    {
        // Get prefab dynamically, based on public fields set from Unity
		// You can use private fields with SerializeField attribute too
        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if (prefab != null)
            {
                return prefab;
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }
	
	public void CloseMenu(KaijuCallMenu menuToClose)
	{
		
		if (menuStack.Count == 0)
		{
			Debug.LogErrorFormat(menuToClose, "{0} cannot be closed because menu stack is empty", menuToClose.GetType());
			return;
		}
		
		// The menu is not outside of the stack and not on top of it, so we cannot remove it
		if (menuToClose.AddToStack && menuStack.Peek() != menuToClose)
			throw new Exception("Trying to remove a menu that is in the stack is unsupported");
		
		// The menu is safe to remove
		var instance = menuToClose.AddToStack ? menuStack.Pop() : menuToClose;

		if (instance.DestroyWhenClosed)
			Destroy(instance.gameObject);
		else
			instance.gameObject.SetActive(false);

		// Re-activate top menu
		// If a re-activated menu is an overlay we need to activate the menu under it
		if (!instance.DisableMenusUnderneath) return;
		
		foreach (var menu in menuStack)
		{
			menu.gameObject.SetActive(true);

			if (menu.DisableMenusUnderneath)
				break;
		}
	}

	public void BackToRoot()
	{
        StaticVariables.menuOpen = false;
        while (menuStack.Count > 1)
		{
			var menu = menuStack.Peek(); 
			
            
			CloseMenu(menu);
		}
	}

    private void Update()
    {
        // On Android the back button is sent as Esc
        if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0)
        {
            menuStack.Peek().OnBackPressed();
        }
    }
}

