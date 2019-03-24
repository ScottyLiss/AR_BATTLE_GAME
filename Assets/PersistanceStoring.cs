using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
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
using Mapbox.Unity.Map;

public class PersistanceStoring : MonoBehaviour
{

    public event VoidDelegate CatalystsChanged;
    
    public Vector3 plyPos;
    public Vector3 worldPos;


    public GameObject player;
    public Location location;

    public Text coords;
    public Text worldpos;
    public Text localPos;
    // Int for zoom = 

    private string filepath = "Assets/Resources/Data.xml";

    // Used to store all values that need to be stored
    public Dictionary<string, Vector3> coordinates = new Dictionary<string, Vector3>();
    public Dictionary<string, Mapbox.Utils.Vector2d> coordinatesPos = new Dictionary<string, Mapbox.Utils.Vector2d>();
    public GameObject[] objectsToStore; // Used to store/hold all the objects into the file (Manually add the objects for now)

    public List<string> tempList = new List<string>();
    public List<Vector3> tempListB = new List<Vector3>();

    XmlDocument mapDoc = new XmlDocument(); // An XmlDocument used to obtain methods like /load etc.. from it
    XmlDocument inventoryDoc = new XmlDocument();


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

    private void InitialiseXmlFile()
    {

        //Note to anyone
        // Current way is a bit of a dire way, it's currently putting both names / coords in different lists, which will be in the same order
        // In other words, [0] in nodelist (name), has [0] in nodelistB (coords)
        if(File.Exists("Data.xml")) 
        {
            mapDoc.Load("Data.xml");
            XmlNodeList nodelist = mapDoc.SelectNodes("Objects/Name");

            foreach (XmlNode node in nodelist)
            {
                tempList.Add(node.InnerText);
                //Debug.Log(node.InnerText); // Displays the name of the object stored
            }

            XmlNodeList nodelistB = mapDoc.SelectNodes("Objects/Coordinate");
            foreach (XmlNode node in nodelistB)
            {
                string tArray;
                if (node.InnerText.StartsWith("(") && node.InnerText.EndsWith(")")) //By default the coords are noted as (x,y,z), here we remove the ( ) 
                {
                    tArray = node.InnerText.Substring(1, node.InnerText.Length - 2);
                    string[] sArray = tArray.Split(','); // Here we remove the ',' between the numbers and pushing them into an array which allows us to create a single vector3 with all the numbers
                    tempListB.Add(new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2])));
                }
            }
            // Load Data 
        }
        else
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create("Data.xml", settings);
            writer.WriteStartDocument();
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
        var catalystStatsAdjustments = new XElement("Stats-Adjustments");

        if (newCatalyst.statsAdjustment.damage != 0)
            catalystStatsAdjustments.Add(new XElement("Damage", newCatalyst.statsAdjustment.damage.ToString()));
        if (newCatalyst.statsAdjustment.maxHealth != 0)
            catalystStatsAdjustments.Add(new XElement("Health", newCatalyst.statsAdjustment.maxHealth.ToString()));
        if (newCatalyst.statsAdjustment.maxStamina != 0)
            catalystStatsAdjustments.Add(new XElement("Stamina", newCatalyst.statsAdjustment.maxStamina.ToString(CultureInfo.InvariantCulture)));
        if (newCatalyst.statsAdjustment.critChance != 0)
            catalystStatsAdjustments.Add(new XElement("Crit-Chance", newCatalyst.statsAdjustment.critChance.ToString()));
        if (newCatalyst.statsAdjustment.critMultiplier != 0)
            catalystStatsAdjustments.Add(new XElement("Crit-Multiplier", newCatalyst.statsAdjustment.critMultiplier.ToString(CultureInfo.InvariantCulture)));
        
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

        catalystsElement?.Add(catalystElement);
        
        // Adjust the last created ID for the catalysts
        catalystsElement.SetElementValue("LastCreatedID", Catalyst.LastCreatedID);
        
        // Render and save the document
        xmlDoc.Save("Catalysts.xml");
        
        // Notify the system that catalysts have changed
        CatalystsChanged?.Invoke();
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
            
                // Add the catalyst to the list
                catalystList.Add(newCatalyst);
            }
        }

        return catalystList.ToArray();
    }

    #endregion
    

    // Check Values to position in Unity, see whether it correlates 
    // Test Build on Memu (Emulator)
    // Create Data File on Mobile device, load/save to file 
    // Each tile has a unique ID representing location 
    // Can use tile ID to translate persistance, whenever ID is spawned, spawn given objects (Dirty, but works)

}