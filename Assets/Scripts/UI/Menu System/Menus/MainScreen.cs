using System;
using UnityEngine;

public class MainScreen : SimpleMenu<MainScreen>
{
	public void OnCallPet()
	{
		StaticVariables.playerScript.CallPet();
	}

    public void OnPlaceBeacon()
    {
	    StaticVariables.playerScript.PlaceBeacon();
	}

    public void OnPressBreach()
    {
	    BreachesMenu.Show();
    }

    public void OnPressPetMenu()
    {
	    PetMenu.Show();
    }

    public void OnPressSettings()
    {
	    throw new Exception("Settings Menu not implemented yet");
    }

	public override void OnBackPressed()
	{
		Application.Quit();
	}
}
