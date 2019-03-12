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
    public Vector3 plyPos;
    public Vector3 worldPos;


    public GameObject player;
    public AbstractMap abstractMap;
    //public DeviceLocationProvider locationProvider;
    public Location location;

    ILocationProvider _locationProvider;
    ILocationProvider locationProvider
    {
        get
        {
            if (_locationProvider == null)
            {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }

            return _locationProvider;
        }
    }

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

    XmlDocument doc = new XmlDocument(); // An XmlDocument used to obtain methods like /load etc.. from it



    private void UpdateDictionary()
    {
        foreach(GameObject g in objectsToStore)
        {
            coordinates.Add(g.name, g.transform.position);
            coordinatesPos.Add(g.name, location.LatitudeLongitude);
        }
    }

    //TODO: Check if built, position is displayed or not, correctly.

    private void Start()
    {
        foreach (KeyValuePair<string, Mapbox.Utils.Vector2d> entry in coordinatesPos)
        {
            Debug.Log(entry.Value);
        }

            UpdateDictionary();
        InitialiseXmlFile();
    }

    private void InitialiseXmlFile()
    {

        //Note to anyone
        // Current way is a bit of a dire way, it's currently putting both names / coords in different lists, which will be in the same order
        // In other words, [0] in nodelist (name), has [0] in nodelistB (coords)
        if(File.Exists("Data.xml")) 
        {
            doc.Load("Data.xml");
            XmlNodeList nodelist = doc.SelectNodes("Objects/Name");
            
            foreach(XmlNode node in nodelist)
            {
                tempList.Add(node.InnerText);
                //Debug.Log(node.InnerText); // Displays the name of the object stored
            }

            XmlNodeList nodelistB = doc.SelectNodes("Objects/Coordinate");
            foreach(XmlNode node in nodelistB)
            {
                string tArray;
                if(node.InnerText.StartsWith("(") && node.InnerText.EndsWith(")")) //By default the coords are noted as (x,y,z), here we remove the ( ) 
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


    private void XmlFileWrite()
    {

    }

    private void XmlFileRead()
    {

    }

    private void Update()
    {
        location = locationProvider.CurrentLocation;
        coords.text = location.LatitudeLongitude.ToString();
        string[] cArray = coords.text.Split(',');
        //worldpos.text = Conversions.LatitudeLongitudeToUnityTilePosition(location.LatitudeLongitude, 255, 255, 4096).ToString();
        //worldpos.text = Conversions.GeoToWorldPosition(double.Parse(cArray[0]), double.Parse(cArray[1]), new Mapbox.Utils.Vector2d(10, 10), (float)2.5).ToString();
        worldpos.text = abstractMap.GeoToWorldPosition(locationProvider.CurrentLocation.LatitudeLongitude).ToString();
        localPos.text = player.transform.position.ToString();
    }
    
   
   
    // Check Values to position in Unity, see whether it correlates 
    // Test Build on Memu (Emulator)
    // Create Data File on Mobile device, load/save to file 
    // Each tile has a unique ID representing location 
    // Can use tile ID to translate persistance, whenever ID is spawned, spawn given objects (Dirty, but works)

}