using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetMenu : SimpleMenu<PetMenu>
{

    public Slider HealthBar;
    public Text BondingText;

    public void OnPressMapButton()
    {
        Close();
    }

    public void OnPressTraitsButton()
    {
        TraitsMenu.Show();
    }
    
    public void OnPressCatalystsButton()
    {
        CatalystsMenu.Show();
    }
    
    public void OnPressFeedButton()
    {
        CatalystsMenu.Show();
    }
    
    public void OnPressHealButton()
    {
        CatalystsMenu.Show();
    }

    private void OnEnable()
    {
        // Set the health bar and make sure it stays accurate
        UpdateHealthBar();
        
        //BondingText.text = StaticVariables.petData.stats
        
        Stats.OnStatsChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        Stats.OnStatsChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        HealthBar.maxValue = StaticVariables.petData.stats.maxHealth;
        HealthBar.value = StaticVariables.petData.stats.health;
    }
}
