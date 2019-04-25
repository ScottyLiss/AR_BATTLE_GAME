using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetMenu : SimpleMenu<PetMenu>
{

    public Slider HealthBar;
    public GameObject FeedButton;
    public Text BondingText;

    public GameObject statsPanel;

    public TextMeshProUGUI MaxHealth;
    public TextMeshProUGUI MaxStamina;
    public TextMeshProUGUI StaminaRegen;
    
    public TextMeshProUGUI Armour;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI CriticalMulti;
    public TextMeshProUGUI CriticalChance;

    public void OnPressMapButton()
    {
        MenuManager.Instance.BackToRoot();
    }

    public void OnInfoPress()
    {
        InfoPetMenu.Show();
    }

    public void OnTapAnimation()
    {
        StaticVariables.petAI.anim.SetTrigger("TriggerAlternativeIdle");
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
        if (FoodsMenu.Instance == null)
        {
            FoodsMenu.Show(FeedButton);
            statsPanel.SetActive(false);
        }
        else
        {
            FoodsMenu.Close();
            statsPanel.SetActive(true);
        }
    }
    
    public void OnPressHealButton()
    {
        CatalystsMenu.Show();
    }

    private void OnEnable()
    {
        // Set the health bar and make sure it stays accurate
        UpdateStats();
        
        //BondingText.text = StaticVariables.petData.stats
        
        Stats.OnStatsChanged += UpdateStats;
    }

    private void OnDisable()
    {
        Stats.OnStatsChanged -= UpdateStats;
    }

    private void UpdateStats()
    {
        HealthBar.maxValue = StaticVariables.petData.stats.maxHealth;
        HealthBar.value = StaticVariables.petData.stats.health;
        
        Damage.text = Math.Round(StaticVariables.petData.stats.damage, 2).ToString(CultureInfo.InvariantCulture);

        MaxHealth.text = Math.Round(StaticVariables.petData.stats.maxHealth, 2).ToString(CultureInfo.InvariantCulture);
        MaxStamina.text = Math.Round(StaticVariables.petData.stats.maxStamina, 2).ToString(CultureInfo.InvariantCulture);
        StaminaRegen.text = Math.Round(StaticVariables.petData.stats.staminaRegen, 2).ToString(CultureInfo.InvariantCulture);
        
        Armour.text = Math.Round(StaticVariables.petData.stats.armour, 2).ToString(CultureInfo.InvariantCulture);
        CriticalMulti.text = Math.Round(StaticVariables.petData.stats.critMultiplier, 2).ToString(CultureInfo.InvariantCulture);
        CriticalChance.text = Math.Round(StaticVariables.petData.stats.critChance, 2).ToString(CultureInfo.InvariantCulture) + "%";
    }
}
