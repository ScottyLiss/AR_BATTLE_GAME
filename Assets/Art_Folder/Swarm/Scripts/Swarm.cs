using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public float health; //Option to hit 3 times to defeat one, they're fast but nimble 
    public bool alive;

    public float speed;




    #region Actions

    private void Dodge() //Once hit, move!
    {

    }

    private void Shoot() // Charge up a shot, fire at the player 
    {

    }

    private void Death()
    {

    }


    #endregion



    #region Abilities

    private void Focus(int laneID) // 40% chance, stop in a lane, aim towards the player, fire (charge up takes 1 second to do)
    {

    }

    private void Split(int laneID) // 20% chance, split up to 2 difference lanes, randomly chosen
    {

    }

    private void RandomSplit() // 40% chance, split up randomly in all given lanes, (player forced to go into the least damage-taking lane
    {

    }

    #endregion
}
