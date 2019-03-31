using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Color traitsBackgroundColor;
    public Color petBackgroundColor;
    public Color equipmentBackgroundColor;
    
    public GameObject petMenu;
    public GameObject invMenu;
    public GameObject traitMenu;
    public GameObject background;
    public GameObject catalystMenu;

    public void ClosePetMenu()
    {
        petMenu.SetActive(false);
    }

    public void OpenInv()
    {
        invMenu.SetActive(true);
    }
    public void CloseInv()
    {
        invMenu.SetActive(false);
    }

    public void ToggleTraits()
    {
        if (traitMenu.activeSelf)
        {
            CloseTraits();
        }

        else
        {
            OpenTraits();
            traitMenu.SetActive(true);
        }
    }

    public void OpenTraits()
    {
        CloseInv();
        background.GetComponent<Image>().color = traitsBackgroundColor;

        traitMenu.SetActive(true);
        Debug.Log(traitMenu.activeSelf);

    }

    public void CloseTraits()
    {
        traitMenu.SetActive(false);
        background.GetComponent<Image>().color = petBackgroundColor;
    }

    public void HealPet()
    {
        StaticVariables.petData.stats.health = Mathf.Clamp(StaticVariables.petData.stats.health + 10, 0, StaticVariables.petData.stats.maxHealth);
    }

    public void LoadFeedPetScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSkillScene()
    {
        //SceneManager.LoadScene(4);
    }

    public void OpenCatalystsMenu()
    {
        catalystMenu.GetComponent<CatalystMenu>().EnableMenu();
    }

    public void CloseCatalystsMenu()
    {
        catalystMenu.GetComponent<CatalystMenu>().DisableMenu();
    }
}
