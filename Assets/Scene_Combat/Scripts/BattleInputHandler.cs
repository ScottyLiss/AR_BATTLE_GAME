using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    #region Variables

    #region Camera Variables
    [Header("<Camera Related Variables>")]
    public CinemachineVirtualCamera normalCamera;
    public CinemachineVirtualCamera shakeyCamera;
    #endregion

    #region Material Variables
    [Header("<Material Related Variables>")]
    [SerializeField] private Material defaultEnemyMaterial;
    [SerializeField] private Material defaultPetMaterial;
    [SerializeField] private Material damagedMaterial;
    #endregion

    #region Healthbars 
    [Header("<Health Related Variables>")]
    public Slider playerHealth;
    public Slider enemyHealth;
    #endregion


    #region Damage Variables
    [Header("<Damage Related Variables>")]
    public float fSecondsToDamageEnemy;
    public float fSecondsToDamagePlayer;

    // The values below are set to whatever the value above is
    private float fDefaultSecondsToDamageEnemy;
    private float fDefaultSecondsToDamagePlayer;

    #endregion
    #endregion

    void Start()
    {

        // Setting default values 
        StaticVariables.isInBattle = true; // Combat = true
        StaticVariables.defaultEnemyMaterial = this.defaultEnemyMaterial; // Assigning Enemy Material
        StaticVariables.defaultPetMaterial = this.defaultPetMaterial; // Assigned Pet Material
        StaticVariables.damagedMaterial = this.damagedMaterial; // Assigning damaged Material
        StaticVariables.battleHandler = this; //Assigning this script as the battlehandler


        // Setting default damage timer
        fDefaultSecondsToDamageEnemy = fSecondsToDamageEnemy;
        fDefaultSecondsToDamagePlayer = fSecondsToDamagePlayer;
        
        // Setting Default player HP values
        playerHealth.maxValue = StaticVariables.petData.stats.maxHealth; 
        playerHealth.value = StaticVariables.petData.stats.health;

    }

    #region Camera Methods
        private IEnumerator RunShakeyCamCoroutine(float time)
	    {
		    StaticVariables.battleHandler.shakeyCamera.MoveToTopOfPrioritySubqueue();

		    yield return new WaitForSeconds(time);

		    StaticVariables.battleHandler.normalCamera.MoveToTopOfPrioritySubqueue();
	    }

	    public void RunShakeyCamera(float time)
	    {
		    StartCoroutine(RunShakeyCamCoroutine(time));
	    }
    #endregion

    #region Scene Loading
        public void LoadMenu()
        {
            Application.LoadLevel("menu");
        }
    #endregion

}
