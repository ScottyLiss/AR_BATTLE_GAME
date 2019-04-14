using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Text;
using UnityEngine;
using Mapbox.Map;
using Mapbox.Unity.Utilities;
using System.Globalization;
using System.Linq;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Location;
using UnityEngine.UI;
using System.Security.Permissions;
using Mapbox.Json;
using Mapbox.Json.Bson;
using Mapbox.Json.Linq;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class PersistanceStoring : MonoBehaviour
{

    public event VoidDelegate CatalystsChanged;
    public Location location;
    public AbstractMap map;

    // Used to store all values that need to be stored
    public Dictionary<string, Vector3> coordinates = new Dictionary<string, Vector3>();
    public Dictionary<string, Mapbox.Utils.Vector2d> coordinatesPos = new Dictionary<string, Mapbox.Utils.Vector2d>();
    public GameObject[] objectsToStore; // Used to store/hold all the objects into the file (Manually add the objects for now)

    public List<string> tempList = new List<string>();
    public List<Vector3> tempListB = new List<Vector3>();

    XmlDocument mapDoc = new XmlDocument(); // An XmlDocument used to obtain methods like /load etc.. from it

    ILocationProvider _locationprovider;
    ILocationProvider locationProvider
    {
        get
        {
            if(_locationprovider == null)
            {
                _locationprovider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }
            return _locationprovider;
        }
    }

    private void UpdateDictionary()
    {
        foreach(GameObject g in objectsToStore)
        {
            if(g != null)
            {
                coordinates.Add(g.name, g.transform.position);
                coordinatesPos.Add(g.name, location.LatitudeLongitude);
            }

        }
    }

    //TODO: Check if built, position is displayed or not, correctly.

    private void Start()
    {
        StaticVariables.persistanceStoring = this;
                
        foreach (KeyValuePair<string, Mapbox.Utils.Vector2d> entry in coordinatesPos)
        {
            Debug.Log(entry.Value);
        }

        UpdateDictionary();
        InitialiseXmlFile();
        LoadLastCatalystId();
    }
    
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
            sVector = sVector.Substring(1, sVector.Length-2);
        }
 
        // Split the items
        string[] sArray = sVector.Split(',');
 
        // Store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));
 
        return result;
    }

    private void InitialiseXmlFile()
    {

        //Note to anyone
        // Current way is a bit of a dire way, it's currently putting both names / coords in different lists, which will be in the same order
        // In other words, [0] in nodelist (name), has [0] in nodelistB (coords)
        if(File.Exists(Application.persistentDataPath + "/Data.xml")) 
        {
//            mapDoc.Load("Data.xml");
//            XmlNodeList nodelist = mapDoc.SelectNodes("Data/Objects/Name");
//
//            foreach (XmlNode node in nodelist)
//            {
//                tempList.Add(node.InnerText);
//                //Debug.Log(node.InnerText); // Displays the name of the object stored
//            }
//
//            XmlNodeList nodelistB = mapDoc.SelectNodes("Data/Objects/Coordinate");
//            foreach (XmlNode node in nodelistB)
//            {
//                string tArray;
//                if (node.InnerText.StartsWith("(") && node.InnerText.EndsWith(")")) //By default the coords are noted as (x,y,z), here we remove the ( ) 
//                {
//                    tArray = node.InnerText.Substring(1, node.InnerText.Length - 2);
//                    string[] sArray = tArray.Split(','); // Here we remove the ',' between the numbers and pushing them into an array which allows us to create a single vector3 with all the numbers
//                    tempListB.Add(new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2])));
//                }
//            }
            // Load Data 
        }
        else
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/Data.xml", settings);
            writer.WriteStartDocument();
            
            writer.WriteStartElement("Data");
            writer.WriteStartElement("Objects");

            foreach (KeyValuePair<string, Vector3> entry in coordinates)
            {
                writer.WriteStartElement("Name");
                writer.WriteString(entry.Key);
                writer.WriteEndElement();
                writer.WriteStartElement("Coordinate");
                writer.WriteString(entry.Value.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
        }

        // Initialize the inventory files
        
        // There is no file to store breaches in the inventory, so create one
        if (!File.Exists(Application.persistentDataPath + "/Breaches.xml"))
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/Breaches.xml", settings);
            writer.WriteStartDocument();
            
            writer.WriteStartElement("Breaches");
            
            writer.WriteStartElement("LastCreatedID");
            writer.WriteString(Breach.LastCreatedID.ToString());
            writer.WriteEndElement();
            
            writer.WriteEndElement();
            
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
        }
        
        // A file is already present, so load in the last created ID for the breaches
        else
        {
            // Load the XML document
            var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Breaches.xml");

            XElement lastCreatedIdNode = xmlDoc.Element("Breaches")?.Element("LastCreatedID");
            
            // Set the last created ID
            if (lastCreatedIdNode != null)
                Breach.LastCreatedID = Convert.ToUInt32(lastCreatedIdNode.Value);
        }
        
        // No file exists for storing catalysts, so create one
        if (!File.Exists(Application.persistentDataPath + "/Catalysts.xml"))
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/Catalysts.xml", settings);
            writer.WriteStartDocument();

            writer.WriteStartElement("Catalysts");
            
            writer.WriteStartElement("LastCreatedID");
            writer.WriteString(Catalyst.LastCreatedID.ToString());
            writer.WriteEndElement();
            
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
        }
        
        // A file is already present, so load in the last created ID for the catalysts
        else
        {
            // Load the XML document
            var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Catalysts.xml");

            XElement lastCreatedIdNode = xmlDoc.Element("Catalysts")?.Element("LastCreatedID");
            
            // Set the last created ID
            if (lastCreatedIdNode != null)
                Catalyst.LastCreatedID = Convert.ToUInt32(lastCreatedIdNode.Value);
        }
        
        // No file exists to store the materials, so create one
        if (!File.Exists(Application.persistentDataPath + "/Materials.xml"))
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/Materials.xml", settings);
            writer.WriteStartDocument();
            
            writer.WriteStartElement("Materials");
            
            writer.WriteStartElement("Biomass");
            writer.WriteString("0");
            writer.WriteEndElement();
            
            writer.WriteStartElement("Metal");
            writer.WriteString("0");
            writer.WriteEndElement();
            
            writer.WriteStartElement("Bonding");
            writer.WriteString("0");
            writer.WriteEndElement();
            
            writer.WriteStartElement("Rock");
            writer.WriteString("0");
            writer.WriteEndElement();
            
            writer.WriteStartElement("Water");
            writer.WriteString("0");
            writer.WriteEndElement();
            
            writer.WriteStartElement("Radioactive");
            writer.WriteString("0");
            writer.WriteEndElement();
            
            writer.WriteEndElement();
            
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
            
            SaveMaterials(new Dictionary<Food, int>()
            {
                { Food.Biomass, 0},
                { Food.Metal, 0},
                { Food.Rock, 0},
                { Food.Water, 0},
                { Food.Radioactive, 0},
                { Food.Bonding, 0},
            });
        }

        #region Old Code
        /*
        doc = XDocument.Load(filepath);
        XElement root = doc.Root;

        using (FileStream writer = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(writer))
            {
                // False is for "StandAlone"
                xmlWriter.WriteStartDocument(false);

                //Writes to the root Xelement name
                xmlWriter.WriteStartElement(root.Name.LocalName);

                foreach (XElement parent in root.Nodes())
                {
                    xmlWriter.WriteStartElement(parent.Name.LocalName);
                    foreach (XElement child in parent.Nodes())
                    {
                        xmlWriter.WriteElementString(child.Name.LocalName, child.Value);
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();

                xmlWriter.Flush();
                xmlWriter.Close();
            }
        }
    }*/


        /*if (File.Exists(filepath))
        {


        string m_Path = Application.dataPath; //Gets the location of the game on the device, ensures it gets the absolute path i.e C:/Desktop/ etc..
        File.SetAttributes(filepath, FileAttributes.Normal);
        FileIOPermission filPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, m_Path + "Assets/Resources/Data.xml");

        using (FileStream fs = new FileStream(filepath, FileMode.Create))
        {
            doc.Load(filepath);
            using (XmlTextWriter writer = new XmlTextWriter(filepath, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                //Write an element (The root)
                writer.WriteStartElement("Objects");

                // Loops through all the objects stored into the dictionary, which then gets written down into the xml file
                // Format: 
                // - Name
                // - Name of Object
                // - Coordinate
                // - Value of Coordinate

                // Example
                // Name
                //      Player
                // Coordinate
                //      349,28,109
                foreach (KeyValuePair<string, Vector3> entry in coordinates)
                {
                    writer.WriteStartElement("Name");
                    writer.WriteString(entry.Key);
                    writer.WriteStartElement("Coordinate");
                    writer.WriteString(entry.Value.ToString());
                    writer.WriteEndElement();
                }


                doc.WriteContentTo(writer);
                writer.Close();
            }
        }
    }*/
        #endregion
    }

    #region Catalyst Saving and Loading
    
    // Save a new catalyst to the inventory
    public void SaveNewCatalyst(Catalyst newCatalyst)
    {
        
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Catalysts.xml");

        var catalystsElement = xmlDoc.Element("Catalysts");
        
        // Generate the catalyst XML representation
        var catalystElement = GenerateCatalystXMLNode(newCatalyst);

        catalystsElement?.Add(catalystElement);
        
        // Adjust the last created ID for the catalysts
        catalystsElement.SetElementValue("LastCreatedID", Catalyst.LastCreatedID);
        
        // Render and save the document
        xmlDoc.Save(Application.persistentDataPath + "/Catalysts.xml");
        
        // Notify the system that catalysts have changed
        CatalystsChanged?.Invoke();
    }

    private XContainer GenerateCatalystXMLNode(Catalyst newCatalyst)
    {

        // If there is no catalyst to generate, return null
        if (newCatalyst == null)
            return null;


        // Create elements for all of the catalyst fields we need to persist
        var catalystElement = new XElement("Catalyst");
        var catalystId = new XElement("ID", newCatalyst.id);
        var catalystLevel = new XElement("Level", newCatalyst.level);
        var catalystName = new XElement("Name", newCatalyst.name);
        var catalystRarity = new XElement("Rarity", (int) newCatalyst.rarity);
        var catalystSlot = new XElement("Slot", (int) newCatalyst.slot);

        // Add the fields to the newly created catalyst element
        catalystElement.Add(catalystId);
        catalystElement.Add(catalystLevel);
        catalystElement.Add(catalystName);
        catalystElement.Add(catalystRarity);
        catalystElement.Add(catalystSlot);

        // Create and store the stats adjustments
        var catalystStatsAdjustments = GenerateStatsXMLElement(newCatalyst.statsAdjustment);

        // Add the stats adjustments to the catalyst element
        catalystElement.Add(catalystStatsAdjustments);

        // Generate the XML elements to describe the catalyst effects
        var catalystEffects = new XElement("Effects");

        newCatalyst.effects.ForEach(effect =>
        {
            var catalystEffect = new XElement("Effect");
            var catalystEffectType = new XElement("Type-Index", effect.typeIndex);
            var catalystEffectRarity = new XElement("Rarity", (int) effect.rarity);

            catalystEffect.Add(catalystEffectType);
            catalystEffect.Add(catalystEffectRarity);

            catalystEffects.Add(catalystEffect);
        });

        // Add in all of the final elements
        catalystElement.Add(catalystEffects);

        return catalystElement;
    }

    // Delete a given catalyst from the persistent inventory by ID
    public void DeleteCatalystFromInventory(uint catalystId)
    {
        mapDoc.Load(Application.persistentDataPath + "/Catalysts.xml");
        XmlNodeList nodelist = mapDoc.SelectNodes("Catalyst");

        if (nodelist != null)
            for (int i = 0; i < nodelist.Count; i++)
            {
                // We've found our node, so delete it
                if (nodelist[i].SelectSingleNode("ID")?.Value == catalystId.ToString())
                {
                    var parentNode = nodelist[i].ParentNode;
                    parentNode?.RemoveChild(nodelist[i]);
                }
            }
        
        mapDoc.Save(Application.persistentDataPath + "/Catalysts.xml");

        // Notify the system that catalysts have changed
        CatalystsChanged?.Invoke();
    }
    
    // Extract the last used ID from the catalysts
    public void LoadLastCatalystId()
    {
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Catalysts.xml");

        var lastCatalystId = xmlDoc.Element("Catalysts")?.Element("LastCreatedID")?.Value;

        Catalyst.LastCreatedID = Convert.ToUInt32(lastCatalystId);
    }

    public Catalyst[] LoadCatalystInventory()
    {
        
        // Load the xml document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Catalysts.xml");
        
        // The list of catalysts
        List<Catalyst> catalystList = new List<Catalyst>();
        
        // Get the catalyst data
        var catalystsData = xmlDoc.Element("Catalysts")?.Elements("Catalyst");
        
        // Generate the catalyst effects
        foreach (XElement catalystData in catalystsData)
        {
            
            // Generate a catalyst from the data and add it to the list
            catalystList.Add(LoadCatalyst(catalystData));
        }

        return catalystList.ToArray();
    }

    // Load a catalyst from an XElement
    private Catalyst LoadCatalyst(XElement catalystData)
    {
        if (catalystData == null)
            return null;
        
        var catalystEffectsData = catalystData.Element("Effects")?.Elements("Effect");
            
        // Create a holder for the catalyst effect
        List<CatalystEffect> newCatalystEffects = new List<CatalystEffect>();

        foreach (XElement catalystEffectData in catalystEffectsData)
        {
            int typeIndex = Convert.ToInt32(catalystEffectData.Element("Type-Index")?.Value);
            Rarities rarity = (Rarities)Convert.ToInt32(catalystEffectData.Element("Rarity")?.Value);

            newCatalystEffects.Add(CatalystFactory.CreateNewCatalystEffect(rarity, 0, typeIndex));
        }

        // Parse the catalyst data
        uint newCatalystId = Convert.ToUInt32(catalystData.Element("ID").Value);
        int newCatalystLevel = Convert.ToInt32(catalystData.Element("Level").Value);
        string newCatalystName = catalystData.Element("Name")?.Value;
        Rarities newCatalystRarity = (Rarities)Convert.ToInt32(catalystData.Element("Rarity").Value);
        PetBodySlot newCatalystSlot = (PetBodySlot)Convert.ToInt32(catalystData.Element("Slot").Value);

        var catalystAttributesData = catalystData.Element("Stats-Adjustments");

        var maxHealthData = catalystAttributesData?.Element("Health")?.Value;
        var armourData = catalystAttributesData?.Element("Armour")?.Value;
        var maxStaminaData = catalystAttributesData?.Element("Stamina")?.Value;
        var staminaRegenData = catalystAttributesData?.Element("Stamina-Regen")?.Value;
        var damageData = catalystAttributesData?.Element("Damage")?.Value;
        var critMultiplierData = catalystAttributesData?.Element("Crit-Multiplier")?.Value;
        var critChanceData = catalystAttributesData?.Element("Crit-Chance")?.Value;
        
        // Create the stat adjustments
        if (catalystAttributesData != null)
        {
            Stats newCatalystStatsAdjustments = new Stats()
            {
                maxHealth = maxHealthData != null ? int.Parse(maxHealthData) : 0,
                maxStamina = maxStaminaData != null ? float.Parse(maxStaminaData, CultureInfo.InvariantCulture) : 0,
                armour = armourData != null ? float.Parse(armourData, CultureInfo.InvariantCulture) : 0,
                staminaRegen = staminaRegenData != null ? float.Parse(staminaRegenData, CultureInfo.InvariantCulture) : 0,
                damage = damageData != null ? int.Parse(damageData) : 0,
                critMultiplier = critMultiplierData != null ? float.Parse(critMultiplierData, CultureInfo.InvariantCulture) : 0,
                critChance = critChanceData != null ? int.Parse(critChanceData) : 0
            };
        
            // Create the new catalyst
            Catalyst newCatalyst = new Catalyst()
            {
                id = newCatalystId,
                level = newCatalystLevel,
                name = newCatalystName,
                rarity = newCatalystRarity,
                slot = newCatalystSlot,
            
                effects = newCatalystEffects,
            
                statsAdjustment = newCatalystStatsAdjustments
            };
        
            // Return the new catalyst
            return newCatalyst;
        }

        return null;
    }

    #endregion

    #region Breaches Saving and Loading
    
    // Load all of the breaches
    public Breach[] LoadBreaches()
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Breaches.xml");
        
        var breachesElement = xmlDoc.Element("Breaches");
        Breach.LastCreatedID = Convert.ToUInt32(breachesElement?.Element("LastCreatedID")?.Value);
        
        // Load in every breach element
        var breachElements = breachesElement?.Elements("Breach");
        
        // Iterate through the breaches, loading each one
        List<Breach> breaches = new List<Breach>();

        foreach (var breachElement in breachElements)
        {
            var newBreach = LoadBreach(breachElement);
            
            breaches.Add(newBreach);
        }

        return breaches.ToArray();
    }
    
    // Load in all of the map breaches
    public GameObject[] LoadMapBreaches()
    {
        
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Data.xml");
        
        // Load the data element
        var dataElement = xmlDoc.Element("Data");

        // Get the breaches element or create it
        var breachesElement = CheckAndCreateElement(dataElement, "Breaches");
        
        // Initialize the gameObject array
        List<GameObject> gameObjects = new List<GameObject>();

        // Get all of the map breaches
        var mapBreaches = breachesElement.Elements("Map-Breach");
        
        // Loop through the Map-Breaches and load them
        foreach (var mapBreach in mapBreaches)
        {
            
            // Load in the prefab
            GameObject prefab = Resources.Load<GameObject>("Breach");
            
            // Create a new game object
            GameObject newBreachGameObject = GameObject.Instantiate(prefab);
            
            // Assign a breach behaviour to the object
            BreachBehaviour breachBehaviour = newBreachGameObject.GetComponent<BreachBehaviour>();
            
            // Generate the breach
            Breach newBreach = LoadBreach(mapBreach.Element("Breach"));
            
            // Assign the breach to the behaviour
            breachBehaviour.BreachToRepresent = newBreach;
            breachBehaviour.Initialize();
            
            // Get the position string
            var positionString = mapBreach.Element("Position").Value;
            
            // Split the items
            string[] positionSplit = positionString.Split(',');
            
            // Convert to Latitude and Longitude
            Vector2d positionLatLong = new Vector2d(Convert.ToDouble(positionSplit[1], CultureInfo.InvariantCulture), Convert.ToDouble(positionSplit[0],
                CultureInfo.InvariantCulture));
            
            // Move the new object in position
            newBreachGameObject.transform.position = map.GeoToWorldPosition(positionLatLong);
            
            // Add the new game object to the list
            gameObjects.Add(newBreachGameObject);
        }

        return gameObjects.ToArray();
    }

    public Breach LoadBreach(XContainer breachElement)
    {
        // Create a new breach
        Breach newBreach = new Breach();
        
        // Load in the last cooldown timestamp and the current timestamp
        var breachLastCooldownTimestamp = Convert.ToDateTime(breachElement.Element("Last-Cooldown-Timestamp")?.Value);
        var currentTimestamp = DateTime.Now;
        
        // Compare the two
        var newCooldown = currentTimestamp - breachLastCooldownTimestamp;

        var lastCooldownSeconds = float.Parse(breachElement.Element("Last-Cooldown")?.Value);
        
        // If the time passed between then and now is more than the cooldown, enable the breach, otherwise, update the cooldown
        if (lastCooldownSeconds - newCooldown.Seconds < 0)
        {
            newBreach.Active = true;
            newBreach.Cooldown = 0;
        }
        else
        {
            newBreach.Active = false;
            newBreach.Cooldown = lastCooldownSeconds - newCooldown.Seconds;
        }
        
        newBreach.id  = Convert.ToUInt32(breachElement.Element("ID")?.Value);
        newBreach.Name  = Convert.ToString(breachElement.Element("Name")?.Value);
        newBreach.Rarity  = (Rarities)Convert.ToInt32(breachElement.Element("Rarity")?.Value);
        newBreach.CurrentTierIndex  = Convert.ToInt32(breachElement.Element("Current-Tier-Index")?.Value);
        newBreach.Level  = Convert.ToInt32(breachElement.Element("Level")?.Value);

        var tiersElement = breachElement.Element("Tiers");
        var tierElements = tiersElement?.Elements("Tier");

        var tiers = new List<BreachTier>();
        
        foreach (var tierElement in tierElements)
        {
            BreachTier breachTier = new BreachTier();

            breachTier.Active = Convert.ToBoolean(tierElement.Element("Active")?.Value);
            breachTier.ParentBreach = newBreach;
            breachTier.RewardRarity = (Rarities) Convert.ToInt32(tierElement.Element("Reward-Rarity")?.Value);
            breachTier.TierRewardType = tierElement.Element("Reward-Rarity")?.Value == "ResourceReward"
                ? typeof(ResourceReward)
                : typeof(CatalystReward);

            breachTier.Encounter = LoadEncounterData(tierElement.Element("Encounter"));
            
            tiers.Add(breachTier);
        }

        newBreach.BreachTiers = tiers;

        return newBreach;
    }
    
    // Save a new breach to the inventory
    public void SaveNewBreach(Breach newBreach)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Breaches.xml");

        var breachesElement = xmlDoc.Element("Breaches");
        
        // Generate the breach XML representation
        var breachElement = GenerateBreachXMLNode(newBreach);

        breachesElement?.Add(breachElement);
        
        // Adjust the last created ID for the catalysts
        breachesElement?.SetElementValue("LastCreatedID", Breach.LastCreatedID);
        
        // Render and save the document
        xmlDoc.Save(Application.persistentDataPath + "/Breaches.xml");
    }
    
    // Save a new breach on the map
    public void SaveNewBreachMap(Breach newBreach, GameObject breachObject)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Data.xml");
        
        // Load the data element
        var dataElement = xmlDoc.Element("Data");

        // Get the breaches element or create it
        var breachesElement = CheckAndCreateElement(dataElement, "Breaches");
        
        // Initialize the breach element
        XContainer breachElement;
        
        // Find all of the map breach elements, or if none exist, make one
        var mapBreachElements = breachesElement.Elements("Map-Breach");
        
        // No other breaches have been saved, so just save this one
        if (!mapBreachElements.Any())
        {
            mapBreachElements = new[] {CheckAndCreateElement(breachesElement, "Map-Breach")};
            
            // Generate the breach XML representation
            breachElement = GenerateBreachXMLNode(newBreach);
            
            mapBreachElements.First().Add(breachElement);
            mapBreachElements.First().Add(new XElement("Position", map.WorldToGeoPosition(breachObject.transform.position)));
        }

        // There are other breaches, so check through to see if there is already a representation of this breach to override
        else
        {
            // Find the representation of this breach
            var filteredMapBreachElements = mapBreachElements.Where(mapBreachElement => Convert.ToUInt32(mapBreachElement.Element("Breach")?.Element("ID")?.Value) == newBreach.id);
            
            // Generate the breach XML representation
            breachElement = GenerateBreachXMLNode(newBreach);
            
            // Create a new map breach element
            var newMapBreachElement = new XElement("Map-Breach");
                
            // Set the breach and the position in it
            newMapBreachElement.Add(breachElement);
            newMapBreachElement.Add(new XElement("Position", map.WorldToGeoPosition(breachObject.transform.position)));
            
            // If there is an element representing this breach, replace it with the newly created element
            if (filteredMapBreachElements.Any())
            {
                
                foreach (var filteredMapBreachElement in filteredMapBreachElements)
                {
                    filteredMapBreachElement.Remove();
                }
            }
            
            // Add it to all of the rest
            breachesElement.Add(newMapBreachElement);
        }
        
        // Render and save the document
        xmlDoc.Save(Application.persistentDataPath + "/Data.xml");
    }

    private XContainer GenerateBreachXMLNode(Breach newBreach)
    {
        
        // If there is no breach to generate, return null
        if (newBreach == null)
            return null;
        
        
        // Create elements for all of the breach fields we need to persist
        var breachElement = new XElement("Breach");
        
        var breachId = new XElement("ID", newBreach.id);
        var breachName = new XElement("Name", newBreach.Name);
        var breachRarity = new XElement("Rarity", (int) newBreach.Rarity);
        var breachCurrentTier = new XElement("Current-Tier-Index", newBreach.CurrentTierIndex);
        var breachLastCooldown = new XElement("Last-Cooldown", newBreach.Cooldown);
        var breachLastCooldownTimestamp = new XElement("Last-Cooldown-Timestamp", DateTime.Now);
        var breachActive = new XElement("Active", newBreach.Active);
        var breachLevel = new XElement("Level", newBreach.Level);
        
        // Add the fields to the newly created catalyst element
        breachElement.Add(breachId);
        breachElement.Add(breachName);
        breachElement.Add(breachRarity);
        breachElement.Add(breachCurrentTier);
        breachElement.Add(breachLastCooldown);
        breachElement.Add(breachLastCooldownTimestamp);
        breachElement.Add(breachActive);
        breachElement.Add(breachLevel);
        
        // Create and store the tiers
        var breachTiers = new XElement("Tiers");
        
        newBreach.BreachTiers.ForEach(tier =>
        {
            breachTiers.Add(GenerateBreachTierElement(tier));
        });
        
        breachElement.Add(breachTiers);

        return breachElement;
    }
    
    private XContainer GenerateBreachTierElement(BreachTier tier)
    {
        
        // Generate the info
        var tierElement = new XElement("Tier");
        
        var tierActive = new XElement("Active", tier.Active);
        var rewardElement = new XElement("Reward-Type", tier.TierRewardType.ToString());
        var rewardRarity = new XElement("Reward-Rarity", ((int) tier.RewardRarity).ToString());
        var encounterElement = GenerateEncounterElement(tier.Encounter);
        
        
        // Add the fields to the newly created tier element
        tierElement.Add(tierActive);
        tierElement.Add(rewardElement);
        tierElement.Add(rewardRarity);
        tierElement.Add(encounterElement);

        return tierElement;
    }

    public void DeleteBreachFromInventory(Breach breach)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Breaches.xml");

        var breachesElement = xmlDoc.Element("Breaches");
        
        // Get all of the breaches
        var breachElements = breachesElement.Elements("Breach");
        
        // Find the breach with the same ID
        var matchingElements = breachElements.Where(breachElement =>
            Convert.ToUInt32(breachElement.Element("ID")?.Value) == breach.id);
        
        // Remove the matching elements
        foreach (var matchingElement in matchingElements)
        {
            matchingElement.Remove();
        }
        
        // Save the changes
        xmlDoc.Save(Application.persistentDataPath + "/Breaches.xml");
    }
    
    public void DeleteBreachFromMap(Breach breach)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Data.xml");
        
        // Load the data element
        var dataElement = xmlDoc.Element("Data");

        var breachesElement = dataElement.Element("Breaches");
        
        // Get all of the breaches
        var mapBreachElements = breachesElement.Elements("Map-Breach");
        
        // Find the breach with the same ID
        var matchingElements = mapBreachElements.Where(mapBreachElement =>
            Convert.ToUInt32(mapBreachElement.Element("Breach").Element("ID")?.Value) == breach.id);
        
        // Remove the matching elements
        foreach (var matchingElement in matchingElements)
        {
            matchingElement.Remove();
        }
        
        // Save the changes
        xmlDoc.Save(Application.persistentDataPath + "/Data.xml");
    }

    #endregion

    #region Rewards Saving and Loading

    private XContainer GenerateRewardElement(Reward reward)
    {
        
        // TODO: Add reward persistence
        return null;
    }

    #endregion

    #region Encounter Saving and Loading

    private CombatEncounter LoadEncounterData(XElement encounterElement)
    {
        CombatEncounter encounter = new CombatEncounter();
        
        EncounterType encounterType = (EncounterType)Convert.ToInt32(encounterElement.Element("Encounter-Type")?.Value);

        encounter.enemyType = encounterType;
        
        encounter.encounterLevel = Convert.ToInt32(encounterElement.Element("Encounter-Level")?.Value);
        
        // See what kind of encounter it is and generate the element
        if (encounterType == EncounterType.Arsenal)
        {
            
            // Translate the info
            ArsenalEncounterInfo newInfo = new ArsenalEncounterInfo();
            
            // Create stat elements for the parts
            var mainBodyStatsElement = encounterElement.Element("Main-Body-Stats");
            newInfo.mainBodyStats = LoadEnemyStatsXMLElement(mainBodyStatsElement);
            
            var upperLeftArmStatsElement = encounterElement.Element("Upper-Left-Arm-Stats");
            newInfo.upperLeftArmStats = LoadEnemyStatsXMLElement(upperLeftArmStatsElement);
            
            var lowerLeftArmStatsElement = encounterElement.Element("Lower-Left-Arm-Stats");
            newInfo.lowerLeftArmStats = LoadEnemyStatsXMLElement(lowerLeftArmStatsElement);
            
            var upperRightArmStatsElement = encounterElement.Element("Upper-Right-Arm-Stats");
            newInfo.upperRightArmStats = LoadEnemyStatsXMLElement(upperRightArmStatsElement);
            
            var lowerRightArmStatsElement = encounterElement.Element("Lower-Right-Arm-Stats");
            newInfo.lowerRightArmStats = LoadEnemyStatsXMLElement(lowerRightArmStatsElement);

            encounter.encounterInfo = newInfo;
        }
        
        else if (encounterType == EncounterType.Swarm)
        {
            
            SwarmEncounterInfo newInfo = new SwarmEncounterInfo();
            
            newInfo.mainBodyStats = LoadEnemyStatsXMLElement(encounterElement);
            
            encounter.encounterInfo = newInfo;
        }
        
        encounter.encounterInfo.Initialize();

        return encounter;
    }

    private XContainer GenerateEncounterElement(CombatEncounter combatEncounter)
    {
        
        // Generate the base element
        var encounterElement = new XElement("Encounter");
        
        // Save the encounter type
        var encounterTypeElement = new XElement("Encounter-Type", (int)combatEncounter.enemyType); 
        var encounterLevelElement = new XElement("Encounter-Level", combatEncounter.encounterLevel); 
        
        encounterElement.Add(encounterLevelElement);
        encounterElement.Add(encounterTypeElement);
        
        // See what kind of encounter it is and generate the element
        if (combatEncounter.enemyType == EncounterType.Arsenal)
        {
            
            // Translate the info
            ArsenalEncounterInfo newInfo = (ArsenalEncounterInfo) combatEncounter.encounterInfo;
            
            // Create stat elements for the parts
            var mainBodyStatsElement = new XElement("Main-Body-Stats");
            mainBodyStatsElement.Add(GenerateEnemyStatsXMLElement(newInfo.mainBodyStats));
            
            var upperLeftArmStatsElement = new XElement("Upper-Left-Arm-Stats");
            upperLeftArmStatsElement.Add(GenerateEnemyStatsXMLElement(newInfo.mainBodyStats));
            
            var lowerLeftArmStatsElement = new XElement("Lower-Left-Arm-Stats");
            lowerLeftArmStatsElement.Add(GenerateEnemyStatsXMLElement(newInfo.mainBodyStats));
            
            var upperRightArmStatsElement = new XElement("Upper-Right-Arm-Stats");
            upperRightArmStatsElement.Add(GenerateEnemyStatsXMLElement(newInfo.mainBodyStats));
            
            var lowerRightArmStatsElement = new XElement("Lower-Right-Arm-Stats");
            lowerRightArmStatsElement.Add(GenerateEnemyStatsXMLElement(newInfo.mainBodyStats));
                     
            // Add the elements to the encounter element
            encounterElement.Add(mainBodyStatsElement);
            encounterElement.Add(upperLeftArmStatsElement);
            encounterElement.Add(lowerLeftArmStatsElement);
            encounterElement.Add(upperRightArmStatsElement);
            encounterElement.Add(lowerRightArmStatsElement);
        }
        
        else if (combatEncounter.enemyType == EncounterType.Swarm)
        {

            SwarmEncounterInfo newInfo = (SwarmEncounterInfo) combatEncounter.encounterInfo;

            var statsAdjustment = GenerateEnemyStatsXMLElement(newInfo.mainBodyStats);
            
            encounterElement.Add(statsAdjustment);
        }

        return encounterElement;
    }

    #endregion

    #region Traits Saving and Loading

    private XContainer GenerateTraitXMLElement(Trait trait)
    {
        
        // Generate the info
        var traitElement = new XElement("Trait");
        
        var name = new XElement("Name", trait.name);
        var activationPoints = new XElement("Activation-Points", trait.ActivationPoints);
        //var bondingCost = new XElement("Bonding-Cost", (int)trait.bondingCost);
        
        
        // Add the fields to the newly created catalyst element
        traitElement.Add(name);
        traitElement.Add(activationPoints);
        traitElement.Add(GenerateStatsXMLElement(trait.statsAdjustment));

        return traitElement;
    }
    
    private XElement GenerateStatsXMLElement(Stats statsAdjustment)
    {
        // Create and store the stats adjustments
        var statsAdjustments = new XElement("Stats-Adjustments");

        if (statsAdjustment.damage != 0)
            statsAdjustments.Add(new XElement("Damage", statsAdjustment.damage.ToString()));
        if (statsAdjustment.maxHealth != 0)
            statsAdjustments.Add(new XElement("Health", statsAdjustment.maxHealth.ToString()));
        if (statsAdjustment.maxStamina != 0)
            statsAdjustments.Add(new XElement("Stamina", statsAdjustment.maxStamina.ToString(CultureInfo.InvariantCulture)));
        if (statsAdjustment.staminaRegen != 0)
            statsAdjustments.Add(new XElement("Stamina-Regen", statsAdjustment.staminaRegen.ToString(CultureInfo.InvariantCulture)));
        if (statsAdjustment.critChance != 0)
            statsAdjustments.Add(new XElement("Crit-Chance", statsAdjustment.critChance.ToString()));
        if (statsAdjustment.critMultiplier != 0)
            statsAdjustments.Add(new XElement("Crit-Multiplier", statsAdjustment.critMultiplier.ToString(CultureInfo.InvariantCulture)));
        if (statsAdjustment.armour != 0)
            statsAdjustments.Add(new XElement("Armour", statsAdjustment.armour.ToString(CultureInfo.InvariantCulture)));
        if (statsAdjustment.staminaRegen != 0)
            statsAdjustments.Add(new XElement("Stamina-Regen", statsAdjustment.staminaRegen.ToString(CultureInfo.InvariantCulture)));
        if (statsAdjustment.staminaDelay != 0)
            statsAdjustments.Add(new XElement("Stamina-Delay", statsAdjustment.staminaDelay.ToString(CultureInfo.InvariantCulture)));
        if (statsAdjustment.staminaCostScaling != 0)
            statsAdjustments.Add(new XElement("Stamina-Cost", statsAdjustment.staminaCostScaling.ToString(CultureInfo.InvariantCulture)));

        return statsAdjustments;
    }
    
    private XElement GenerateEnemyStatsXMLElement(EnemyStats statsAdjustment)
    {
        // Create and store the stats adjustments
        var statsAdjustments = new XElement("Stats-Adjustments");

        if (statsAdjustment.Damage != 0)
            statsAdjustments.Add(new XElement("Damage", statsAdjustment.Damage.ToString()));
        if (statsAdjustment.MaxHealth != 0)
            statsAdjustments.Add(new XElement("Health", statsAdjustment.MaxHealth.ToString()));
        if (statsAdjustment.Armour != 0)
            statsAdjustments.Add(new XElement("Armour", statsAdjustment.Armour.ToString()));
        if (statsAdjustment.AttackSpeed != 0)
            statsAdjustments.Add(new XElement("Attack-Speed", statsAdjustment.AttackSpeed.ToString(CultureInfo.InvariantCulture)));
        if (statsAdjustment.StaggerResist != 0)
            statsAdjustments.Add(new XElement("Stagger-Resist", statsAdjustment.StaggerResist.ToString(CultureInfo.InvariantCulture)));

        return statsAdjustments;
    }
    
    private Stats LoadStatsXMLElement(XElement statsAdjustmentData)
    {
        // Create and store the stats adjustments
        Stats statsAdjustments = new Stats();

        statsAdjustments.damage = float.Parse(statsAdjustmentData.Element("Damage")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.maxHealth = float.Parse(statsAdjustmentData.Element("Health")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.maxStamina = float.Parse(statsAdjustmentData.Element("Stamina")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.staminaRegen = float.Parse(statsAdjustmentData.Element("Stamina-Regen")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.critChance = float.Parse(statsAdjustmentData.Element("Crit-Chance")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.critMultiplier = float.Parse(statsAdjustmentData.Element("Crit-Multiplier")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.armour = float.Parse(statsAdjustmentData.Element("Armour")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.staminaRegen = float.Parse(statsAdjustmentData.Element("Stamina-Regen")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.staminaDelay = float.Parse(statsAdjustmentData.Element("Stamina-Delay")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.staminaCostScaling = float.Parse(statsAdjustmentData.Element("Stamina-Cost")?.Value ?? "0", CultureInfo.InvariantCulture);

        return statsAdjustments;
    }
    
    private EnemyStats LoadEnemyStatsXMLElement(XElement statsAdjustmentData)
    {
        // Create and store the stats adjustments
        EnemyStats statsAdjustments = new EnemyStats();

        var statsAdjustmentsElement = statsAdjustmentData.Element("Stats-Adjustments");

        statsAdjustments.Damage = Convert.ToInt32(statsAdjustmentsElement.Element("Damage")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.MaxHealth = Convert.ToInt32(statsAdjustmentsElement.Element("Health")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.Health = statsAdjustments.MaxHealth;
        statsAdjustments.Armour = Convert.ToInt32(statsAdjustmentsElement.Element("Armour")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.AttackSpeed = float.Parse(statsAdjustmentsElement.Element("Attack-Speed")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.StaggerResist = Convert.ToInt32(statsAdjustmentsElement.Element("Stagger-Resist")?.Value ?? "0", CultureInfo.InvariantCulture);

        return statsAdjustments;
    }

    #endregion

    #region Pet Saving and Loading
    
    // Save the pet data in the data xml
    public void SavePetData()
    {
        // Save the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Data.xml");
        
        // Save in the data root element
        var rootElement = CheckAndCreateElement(xmlDoc, "Data");
        
        // Save in the pet data
        var petDataElement =  CheckAndCreateElement(rootElement,"Pet-Data");
        
        // Save the pet position
        CheckAndCreateElement(petDataElement, "Position", map.WorldToGeoPosition(StaticVariables.petAI.gameObject.transform.position));
        
        // Save the pet level
        CheckAndCreateElement(petDataElement, "Level", StaticVariables.petData.level);
        
        // Save in the stats
        var petStats = CheckAndCreateElement(petDataElement, "Stats");
        
        // Save in the stats
        CheckAndCreateElement(petStats, "Max-Health",      StaticVariables.petData.stats.maxHealth);
        CheckAndCreateElement(petStats, "Health",          StaticVariables.petData.stats.health);
        CheckAndCreateElement(petStats, "Stamina",         StaticVariables.petData.stats.maxStamina);
        CheckAndCreateElement(petStats, "Armour",          StaticVariables.petData.stats.armour);
        CheckAndCreateElement(petStats, "Stamina-Regen",   StaticVariables.petData.stats.staminaRegen);
        CheckAndCreateElement(petStats, "Damage",          StaticVariables.petData.stats.damage);
        CheckAndCreateElement(petStats, "Crit-Multiplier", StaticVariables.petData.stats.critMultiplier);
        CheckAndCreateElement(petStats, "Crit-Chance",     StaticVariables.petData.stats.critChance);
        CheckAndCreateElement(petStats, "Hunger",          StaticVariables.petData.hunger);
        
        // Save in the catalyst holder
        var catalystsElement = CheckAndCreateElement(petDataElement, "Catalysts");
        
        // Generate the XML elements for the catalysts
        CheckAndCreateElement(catalystsElement, "Head", GenerateCatalystXMLNode(StaticVariables.petData.headCatalyst));
        CheckAndCreateElement(catalystsElement, "Body", GenerateCatalystXMLNode(StaticVariables.petData.bodyCatalyst));
        CheckAndCreateElement(catalystsElement, "Tail", GenerateCatalystXMLNode(StaticVariables.petData.tailCatalyst));
        CheckAndCreateElement(catalystsElement, "Legs", GenerateCatalystXMLNode(StaticVariables.petData.legsCatalyst));
        
        // Save in the traits holder
        var traitsElement = CheckAndCreateElement(petDataElement, "Traits");
        
        // Clear the previous trait state
        traitsElement.RemoveAll();
        
        // Load in all of the traits
        foreach ( var traitValuePair in StaticVariables.traitManager.allTraits)
        {
            traitsElement.Add(GenerateTraitXMLElement(traitValuePair.Value));
        }
        
        // Render and save the document
        xmlDoc.Save(Application.persistentDataPath + "/Data.xml");
        
        // Notify the system that catalysts have changed
        CatalystsChanged?.Invoke();
    }

    public void LoadPetData(PetData petData)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Data.xml");
        
        // Load in the data root element
        var rootElement = CheckAndCreateElement(xmlDoc, "Data");
        
        // Load in the pet data
        var petDataElement =  CheckAndCreateElement(rootElement,"Pet-Data");
        
        // Load in the pet position
        string positionString = CheckAndCreateElement(petDataElement, "Position")?.Value;

        int newLevel = 1;

        if (!string.IsNullOrEmpty(positionString))
        {
            // Split the items
            string[] positionSplit = positionString.Split(',');
        
            // Convert to Latitude and Longitude
            Vector2d positionLatLong = new Vector2d(Convert.ToDouble(positionSplit[1], CultureInfo.InvariantCulture), Convert.ToDouble(positionSplit[0],
                CultureInfo.InvariantCulture));
        
            // Convert to world position and set
            map.OnInitialized += () =>
            {
                StaticVariables.petAI.gameObject.transform.position = map.GeoToWorldPosition(positionLatLong);
                StaticVariables.petAI.GetComponent<Arrival>().targetPosition = map.GeoToWorldPosition(positionLatLong);
            };
        }

        if (petDataElement.Element("Stats") != null)
        {
            
            // Load the pet stats
            var petStats = CheckAndCreateElement(petDataElement, "Stats");
            
            // Set the pet stats
            petData.stats.maxHealth = Convert.ToInt32(CheckAndCreateElement(petStats, "Max-Health").Value);
            petData.stats.health = float.Parse(CheckAndCreateElement(petStats, "Health").Value,
                CultureInfo.InvariantCulture);
            petData.stats.armour = Convert.ToInt32(CheckAndCreateElement(petStats, "Armour").Value);
            petData.stats.maxStamina = float.Parse(CheckAndCreateElement(petStats, "Stamina").Value,
                CultureInfo.InvariantCulture);
            petData.stats.stamina = float.Parse(CheckAndCreateElement(petStats, "Stamina").Value,
                CultureInfo.InvariantCulture);
            petData.stats.staminaRegen = float.Parse(CheckAndCreateElement(petStats, "Stamina-Regen").Value,
                CultureInfo.InvariantCulture);
            petData.stats.damage = float.Parse(CheckAndCreateElement(petStats, "Damage").Value);
            petData.stats.critChance = float.Parse(CheckAndCreateElement(petStats, "Crit-Chance").Value);
            petData.stats.critMultiplier = float.Parse(CheckAndCreateElement(petStats, "Crit-Multiplier").Value,
                CultureInfo.InvariantCulture);
            
            petData.hunger = float.Parse(CheckAndCreateElement(petStats, "Hunger").Value,
                CultureInfo.InvariantCulture);
            
            // Load in the pet level
            newLevel = Convert.ToInt32(CheckAndCreateElement(petDataElement, "Level")?.Value);
        }

        else
        {
            var baseStats = LoadPetBaseStats();
            
            petData.stats.maxHealth = baseStats.maxHealth; 
            petData.stats.health = baseStats.health; 
            petData.stats.armour = baseStats.armour; 
            petData.stats.maxStamina = baseStats.maxStamina; 
            petData.stats.damage = baseStats.damage; 
            petData.stats.critChance = baseStats.critChance; 
            petData.stats.critMultiplier = baseStats.critMultiplier; 
            petData.stats.stamina = baseStats.maxStamina; 
            petData.stats.staminaRegen = baseStats.staminaRegen;
            petData.hunger = 0;
        }
        
        // Set the level
        petData.level = newLevel;

        // Set the pet catalysts
        petData.headCatalyst = LoadCatalyst(petDataElement.Element("Catalysts")?.Element("Head")?.Element("Catalyst"));
        petData.bodyCatalyst = LoadCatalyst(petDataElement.Element("Catalysts")?.Element("Body")?.Element("Catalyst"));
        petData.tailCatalyst = LoadCatalyst(petDataElement.Element("Catalysts")?.Element("Tail")?.Element("Catalyst"));
        petData.legsCatalyst = LoadCatalyst(petDataElement.Element("Catalysts")?.Element("Legs")?.Element("Catalyst"));
    }

    public Trait LoadTraitData(XElement traitData)
    {
        Trait newTrait = ScriptableObject.CreateInstance<Trait>();

        newTrait.name = traitData.Element("Name")?.Value;
        newTrait.activationPoints = Convert.ToInt32(traitData.Element("Activation-Points")?.Value);
        newTrait.statsAdjustment = LoadStatsXMLElement(traitData.Element("Stats-Adjustments"));

        return newTrait;
    }

    public void LoadTraitsData(Dictionary<string, Trait> traits)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Data.xml");
        
        // Load in the data root element
        var rootElement = CheckAndCreateElement(xmlDoc, "Data");
        
        // Load in the data root element
        var petDataElement = CheckAndCreateElement(rootElement, "Pet-Data");
        
        // Load in the traits element
        var traitsDataElement = CheckAndCreateElement(petDataElement, "Traits");
        
        // Check if we have traits
        if (traitsDataElement.Element("Trait") == null)
        {
            return;
        }
        
        // Load in all of the traits
        var traitsElements = traitsDataElement.Elements("Trait");
        
        foreach (XElement traitsElement in traitsElements)
        {
            Trait trait = traits[traitsElement.Element("Name")?.Value];

            trait.activationPoints = Convert.ToInt32(traitsElement.Element("Activation-Points")?.Value);
            trait.statsAdjustment = LoadStatsXMLElement(traitsElement.Element("Stats-Adjustments"));

            trait.CheckActive();
        }
    }

    #endregion

    #region Materials Saving and Loading

    public void SaveMaterials(Dictionary<Food, int> resources)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Materials.xml");
        
        // Save in the data root element
        var rootElement = CheckAndCreateElement(xmlDoc, "Materials");
        
        // Load in the values
        CheckAndCreateElement(rootElement, "Biomass", resources[Food.Biomass]);
        CheckAndCreateElement(rootElement, "Water", resources[Food.Water]);
        CheckAndCreateElement(rootElement, "Metal", resources[Food.Metal]);
        CheckAndCreateElement(rootElement, "Radioactive", resources[Food.Radioactive]);
        CheckAndCreateElement(rootElement, "Rock", resources[Food.Rock]);
        CheckAndCreateElement(rootElement, "Bonding", resources[Food.Bonding]);
        
        xmlDoc.Save(Application.persistentDataPath + "/Materials.xml");
    }

    public Dictionary<Food, int> LoadMaterials()
    {
        
        Dictionary<Food, int> foodDictionary = new Dictionary<Food, int>();
        
        // Load the XML document
        var xmlDoc = XDocument.Load(Application.persistentDataPath + "/Materials.xml");
        
        // Save in the data root element
        var rootElement = CheckAndCreateElement(xmlDoc, "Materials");
        
        // Load in the values
        foodDictionary.Add(Food.Biomass, Convert.ToInt32(rootElement.Element("Biomass").Value));
        foodDictionary.Add(Food.Water, Convert.ToInt32(rootElement.Element("Water").Value));
        foodDictionary.Add(Food.Metal, Convert.ToInt32(rootElement.Element("Metal").Value));
        foodDictionary.Add(Food.Radioactive, Convert.ToInt32(rootElement.Element("Radioactive").Value));
        foodDictionary.Add(Food.Rock, Convert.ToInt32(rootElement.Element("Rock").Value));
        foodDictionary.Add(Food.Bonding, Convert.ToInt32(rootElement.Element("Bonding").Value));

        return foodDictionary;
    }

    #endregion

    #region Designer Definition Files Loading
    
    // Load in the base stats for the pet
    public Stats LoadPetBaseStats()
    {
        // Load in the designer definitions file      
        string definitionsString;

        var definitionsAsset = Resources.Load<TextAsset>("DesignerDefinitions");

        definitionsString = definitionsAsset.text;
        
        // Parse the text into an object
        JObject definitionsObject = JObject.Parse(definitionsString);
        
        // Save the token with the relevant stats
        JToken relevantPetStats = definitionsObject["PetData"]["Base-Stats"];
        
        // Set all of the stats into a new object
        Stats newPetBaseStats = new Stats()
        {
            maxHealth = relevantPetStats["Health"].Value<int>(),
            health = relevantPetStats["Health"].Value<int>(),
            armour = relevantPetStats["Armour"].Value<float>(),
            damage = relevantPetStats["Damage"].Value<int>(),
            critChance = relevantPetStats["CritChance"].Value<float>(),
            critMultiplier = relevantPetStats["CritMulti"].Value<float>(),
            maxStamina = relevantPetStats["StaminaMax"].Value<float>(),
            staminaRegen = relevantPetStats["StaminaRegen"].Value<float>(),
            stamina = relevantPetStats["StaminaMax"].Value<float>(),
            staminaDelay = relevantPetStats["StaminaRegenDelay"].Value<float>(),
            staminaCostScaling = relevantPetStats["StaminacostScaling"].Value<float>(),
        };

        return newPetBaseStats;
    }
    
    // Load in the base stat of a specific type of enemy
    public EnemyStats LoadEnemyBaseStats(string enemyType)
    {
        // Load in the designer definitions file      
        string definitionsString;

        var definitionsAsset = Resources.Load<TextAsset>("DesignerDefinitions");

        definitionsString = definitionsAsset.text;
        
        // Parse the text into an object
        JObject definitionsObject = JObject.Parse(definitionsString);
        
        // Save the token with the relevant stats
        JToken relevantEnemyStats = definitionsObject["Combat"][enemyType]["Base"];
        
        // Set all of the stats into a new object
        EnemyStats newEnemyStats = new EnemyStats()
        {
            MaxHealth = Convert.ToInt32(relevantEnemyStats["Health"]),
            Health = Convert.ToInt32(relevantEnemyStats["Health"]),
            Armour = Convert.ToInt32(relevantEnemyStats["Armour"]),
            Damage = Convert.ToInt32(relevantEnemyStats["Damage"]),
            AttackSpeed = float.Parse(relevantEnemyStats["AttackSpeed"].ToString()),
            StaggerResist = Convert.ToInt32(relevantEnemyStats["StaggerResist"]),
        };

        return newEnemyStats;
    }
    
    // Load in the scaling of a specific type of enemy
    public EnemyStats LoadEnemyScaling(string enemyType)
    {
        // Load in the designer definitions file      
        string definitionsString;

        var definitionsAsset = Resources.Load<TextAsset>("DesignerDefinitions");

        definitionsString = definitionsAsset.text;
        
        // Parse the text into an object
        JObject definitionsObject = JObject.Parse(definitionsString);
        
        // Save the token with the relevant stats
        JToken relevantEnemyStats = definitionsObject["Combat"][enemyType]["Scaling"];
        
        // Set all of the stats into a new object
        EnemyStats newEnemyStats = new EnemyStats()
        {
            MaxHealth = (int)(Convert.ToInt32(relevantEnemyStats["Health"]) * 0.01),
            Health = (int)(Convert.ToInt32(relevantEnemyStats["Health"]) * 0.01),
            Armour = (int)(Convert.ToInt32(relevantEnemyStats["Armour"])* 0.01),
            Damage = (int)(Convert.ToInt32(relevantEnemyStats["Damage"])* 0.01),
            AttackSpeed = float.Parse(relevantEnemyStats["AttackSpeed"].ToString()),
            StaggerResist = (int)(Convert.ToInt32(relevantEnemyStats["StaggerResist"])* 0.01),
        };

        return newEnemyStats;
    }

    #endregion
    
    private XElement CheckAndCreateElement(XContainer elementToCheck, string nodeName, object value = null)
    {
        
        // Check if one does not exist and add it
        if (elementToCheck.Element(nodeName) == null)
        {
            if (value == null)
            {
                elementToCheck.Add(new XElement(nodeName));
            }

            else
            {
                elementToCheck.Add(new XElement(nodeName, value));
            }
        }
        
        else if (elementToCheck.Element(nodeName) != null && value != null)
        {
            elementToCheck.Element(nodeName).Remove();
            elementToCheck.Add(new XElement(nodeName, value));
        }
        
        // Once it is created, return the new element
        return elementToCheck.Element(nodeName);
    }
    

    // Check Values to position in Unity, see whether it correlates 
    // Test Build on Memu (Emulator)
    // Create Data File on Mobile device, load/save to file 
    // Each tile has a unique ID representing location 
    // Can use tile ID to translate persistance, whenever ID is spawned, spawn given objects (Dirty, but works)
}