
using UnityEngine;

public class BreachReward : Reward
{
    public Rarities rarity;
    public int level;

    public Breach breachToAward;
    
    public override void Award()
    {
        StaticVariables.persistanceStoring.SaveNewBreach(breachToAward);
        // StaticVariables.petData.breachesInventoryTemp.Add(breachToAward);
    }

    public override void SpawnAwardOnMap(Vector3 position)
    {
        // Spawn the catalyst on the map
    }

    public override GameObject MapRepresentation { get; }
    public override GameObject UiRepresentation { get; }
    public override GameObject SpawnUIRepresentation()
    {
        GameObject representationPrefab = Resources.Load<GameObject>("UI/Prefabs/BreachRepresentationUI");

        GameObject newRepresentationInstance = GameObject.Instantiate(representationPrefab);
        
        newRepresentationInstance.GetComponent<BreachViewer>().RepresentBreach(breachToAward);

        return newRepresentationInstance;
    }

    public override GameObject SpawnMapRepresentation()
    {
        throw new System.NotImplementedException();
    }
}