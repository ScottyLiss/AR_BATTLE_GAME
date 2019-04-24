using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JunkPileMenu : KaijuCallMenu<JunkPileMenu> {
	
	// The amount of taps it takes to open a pile
	private const int INTERACTIONS_NEEDED = 4;
	
	// The pile associated with the open junk pile
	private JunkPile junkPile;
	
	// The map pile object that was pressed
	private Vector3 pressedPilePosition;
	
	// The number of times the pile was tapped
	private int interactionCount = 0;
	
	// The reward currently to be viewed
	private int rewardIndex = 0;
	private Reward currentReward = null;

	public GameObject PileUI;
	private GameObject RewardUI;
	public GameObject RewardPlaceholder;

    public AudioClip[] audioClips;
    public AudioSource audioSource;

	public void OnPressedPile()
	{
        audioSource.PlayOneShot(audioClips[interactionCount]);
        // Track the number of times the player has tapped
        interactionCount++;
		
		// Make the graphical change
		PileUI.transform.GetChild(1).localScale = new Vector3(1 - (((float) 1 / INTERACTIONS_NEEDED) * interactionCount) / 2, y: 1 -(((float)1 / INTERACTIONS_NEEDED) * interactionCount) / 2, z: 1);
		PileUI.transform.GetChild(0).localScale = new Vector3(0.5f + (((float) 1 / INTERACTIONS_NEEDED) * interactionCount) / 2, 0.5f + (((float) 1 / INTERACTIONS_NEEDED) * interactionCount) / 2, 1);
		PileUI.transform.GetChild(0).GetComponent<Image>().color = CatalystFactory.rarityColors[(int) junkPile.rarity];

		// If the player hasn't tapped enough times, just apply the visual effect of the pile getting smaller
		if (interactionCount < INTERACTIONS_NEEDED)
		{
			return;
		}
		
		// The player has tapped enough, so destroy the pile object, play the animation and show the rewards
		Destroy(PileUI);
		
		// Show the reward on screen
		ShowNextReward();
	}

	public void ShowNextReward()
	{
		
		// If we're currently looking at a reward, award it and move on to the next one
		currentReward?.SpawnAwardOnMap(pressedPilePosition);

        // There are no more rewards to show, so close the menu
        if (rewardIndex >= junkPile.rewards.Count)
		{
            audioSource.PlayOneShot(audioClips[3]);
            StaticVariables.menuOpen = false;
            //Close();
            MenuManager.Instance.BackToRoot();
            return;
		}
		
		// Set which reward we're currently looking at
		currentReward = junkPile.rewards[rewardIndex];
		rewardIndex++;

		
		// Instantiate the reward's UI representation on screen and delete the old reward UI
		Destroy(RewardUI);
		RewardUI = currentReward.SpawnUIRepresentation();
		RewardUI.transform.SetParent(RewardPlaceholder.transform);
		RewardUI.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		RewardUI.GetComponent<RectTransform>().localScale = Vector3.one;
		
		// Create a new event trigger for the button and hook up the logic
		EventTrigger trigger = RewardUI.GetComponent<EventTrigger>();

		if (trigger == null) trigger = RewardUI.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();

		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((data) => { OnPressedReward(); });
		trigger.triggers.Add(entry);
	}

	public void OnPressedReward()
	{
        currentReward.Award();
		ShowNextReward();
	}

	public static void Show(GameObject pilePressed)
	{
		Open();
        Debug.Log("Show" + pilePressed.name);
		Instance.pressedPilePosition = pilePressed.transform.position;
		Instance.junkPile = JunkPileFactory.GenerateJunkPile();
	}
}
