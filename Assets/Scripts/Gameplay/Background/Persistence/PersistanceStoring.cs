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
        if(File.Exists("Data.xml")) 
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

            XmlWriter writer = XmlWriter.Create("Data.xml", settings);
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
        if (!File.Exists("Breaches.xml"))
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create("Breaches.xml", settings);
            writer.WriteStartDocument();
            
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
        }
        
        // No file exists for storing catalysts, so create one
        if (!File.Exists("Catalysts.xml"))
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create("Catalysts.xml", settings);
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
            XmlDocument catalystDocument = new XmlDocument();
            catalystDocument.Load("Catalysts.xml");
            XmlNode lastCreatedIdNode = catalystDocument.SelectSingleNode("LastCreatedID");
            
            // Set the last created ID
            if (lastCreatedIdNode != null)
                Catalyst.LastCreatedID = Convert.ToUInt32(lastCreatedIdNode.InnerText);
        }
        
        // No file exists to store the materials, so create one
        if (!File.Exists("Materials.xml"))
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create("Materials.xml", settings);
            writer.WriteStartDocument();

            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
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
        var xmlDoc = XDocument.Load("Catalysts.xml");

        var catalystsElement = xmlDoc.Element("Catalysts");
        
        // Generate the catalyst XML representation
        var catalystElement = GenerateCatalystXMLNode(newCatalyst);

        catalystsElement?.Add(catalystElement);
        
        // Adjust the last created ID for the catalysts
        catalystsElement.SetElementValue("LastCreatedID", Catalyst.LastCreatedID);
        
        // Render and save the document
        xmlDoc.Save("Catalysts.xml");
        
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
        var catalystName = new XElement("Name", newCatalyst.name);
        var catalystRarity = new XElement("Rarity", (int) newCatalyst.rarity);
        var catalystSlot = new XElement("Slot", (int) newCatalyst.slot);
        
        // Add the fields to the newly created catalyst element
        catalystElement.Add(catalystId);
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
            var catalystEffectRarity = new XElement("Rarity", (int)effect.rarity);

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
        mapDoc.Load("Catalysts.xml");
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
        
        mapDoc.Save("Catalysts.xml");

        // Notify the system that catalysts have changed
        CatalystsChanged?.Invoke();
    }
    
    // Extract the last used ID from the catalysts
    public void LoadLastCatalystId()
    {
        var xmlDoc = XDocument.Load("Catalysts.xml");

        var lastCatalystId = xmlDoc.Element("Catalysts")?.Element("LastCreatedID")?.Value;

        Catalyst.LastCreatedID = Convert.ToUInt32(lastCatalystId);
    }

    public Catalyst[] LoadCatalystInventory()
    {
        
        // Load the xml document
        var xmlDoc = XDocument.Load("Catalysts.xml");
        
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
        string newCatalystName = catalystData.Element("Name")?.Value;
        Rarities newCatalystRarity = (Rarities)Convert.ToInt32(catalystData.Element("Rarity").Value);
        PetBodySlot newCatalystSlot = (PetBodySlot)Convert.ToInt32(catalystData.Element("Slot").Value);

        var catalystAttributesData = catalystData.Element("Stats-Adjustments");

        var maxHealthData = catalystAttributesData?.Element("Health")?.Value;
        var maxStaminaData = catalystAttributesData?.Element("Stamina")?.Value;
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
                damage = damageData != null ? int.Parse(damageData) : 0,
                critMultiplier = critMultiplierData != null ? float.Parse(critMultiplierData, CultureInfo.InvariantCulture) : 0,
                critChance = critChanceData != null ? int.Parse(critChanceData) : 0
            };
        
            // Create the new catalyst
            Catalyst newCatalyst = new Catalyst()
            {
                id = newCatalystId,
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
    
    private Stats LoadStatsXMLElement(XElement statsAdjustmentData)
    {
        // Create and store the stats adjustments
        Stats statsAdjustments = new Stats();

        statsAdjustments.damage = float.Parse(statsAdjustmentData.Element("Damage")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.maxHealth = float.Parse(statsAdjustmentData.Element("Health")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.maxStamina = float.Parse(statsAdjustmentData.Element("Stamina")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.critChance = float.Parse(statsAdjustmentData.Element("Crit-Chance")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.critMultiplier = float.Parse(statsAdjustmentData.Element("Crit-Multiplier")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.armour = float.Parse(statsAdjustmentData.Element("Armour")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.staminaRegen = float.Parse(statsAdjustmentData.Element("Stamina-Regen")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.staminaDelay = float.Parse(statsAdjustmentData.Element("Stamina-Delay")?.Value ?? "0", CultureInfo.InvariantCulture);
        statsAdjustments.staminaCostScaling = float.Parse(statsAdjustmentData.Element("Stamina-Cost")?.Value ?? "0", CultureInfo.InvariantCulture);

        return statsAdjustments;
    }

    #endregion

    #region Pet Saving and Loading
    
    // Save the pet data in the data xml
    public void SavePetData()
    {
        // Save the XML document
        var xmlDoc = XDocument.Load("Data.xml");
        
        // Save in the data root element
        var rootElement = CheckAndCreateElement(xmlDoc, "Data");
        
        // Save in the pet data
        var petDataElement =  CheckAndCreateElement(rootElement,"Pet-Data");
        
        // Save the pet position
        CheckAndCreateElement(petDataElement, "Position", map.WorldToGeoPosition(StaticVariables.petAI.gameObject.transform.position));
        
        // Save in the stats
        var petStats = CheckAndCreateElement(petDataElement, "Stats");
        
        // Save in the stats
        CheckAndCreateElement(petStats, "Health",          StaticVariables.petData.stats.maxHealth);
        CheckAndCreateElement(petStats, "Stamina",         StaticVariables.petData.stats.maxStamina);
        CheckAndCreateElement(petStats, "Damage",          StaticVariables.petData.stats.damage);
        CheckAndCreateElement(petStats, "Crit-Multiplier", StaticVariables.petData.stats.critMultiplier);
        CheckAndCreateElement(petStats, "Crit-Chance",     StaticVariables.petData.stats.critChance);
        
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
        xmlDoc.Save("Data.xml");
        
        // Notify the system that catalysts have changed
        CatalystsChanged?.Invoke();
    }

    public void LoadPetData(PetData petData)
    {
        // Load the XML document
        var xmlDoc = XDocument.Load("Data.xml");
        
        // Load in the data root element
        var rootElement = CheckAndCreateElement(xmlDoc, "Data");
        
        // Load in the pet data
        var petDataElement =  CheckAndCreateElement(rootElement,"Pet-Data");
        
        // Load in the pet position
        string positionString = CheckAndCreateElement(petDataElement, "Position").Value;
        
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
        
        // Load the pet stats
        var petStats = CheckAndCreateElement(petDataElement, "Stats");
        
        // Set the pet stats
        petData.stats.maxHealth = Convert.ToInt32(CheckAndCreateElement(petStats, "Health").Value);
        petData.stats.maxStamina = float.Parse(CheckAndCreateElement(petStats, "Stamina").Value,
            CultureInfo.InvariantCulture);
        petData.stats.damage = Convert.ToInt32(CheckAndCreateElement(petStats, "Damage").Value);
        petData.stats.critChance = Convert.ToInt32(CheckAndCreateElement(petStats, "Crit-Chance").Value);
        petData.stats.critMultiplier = float.Parse(CheckAndCreateElement(petStats, "Crit-Multiplier").Value,
            CultureInfo.InvariantCulture);
        
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
        var xmlDoc = XDocument.Load("Data.xml");
        
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
        }
    }

    #endregion

    #region Designer Definition Files Loading
    
    // Load in the base stats for the pet
    public Stats LoadPetBaseStats()
    {
        // Load in the designer definitions file      
        string definitionsString = File.ReadAllText(@".\\DesignerDefinitions.json", Encoding.UTF8);
        
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
            critChance = relevantPetStats["CritChance"].Value<int>(),
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
        string definitionsString = File.ReadAllText(@".\\DesignerDefinitions.json", Encoding.UTF8);
        
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
        string definitionsString = File.ReadAllText(@".\\DesignerDefinitions.json", Encoding.UTF8);
        
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