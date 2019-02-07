//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;
//
//[CustomEditor(typeof(TraitBadgeUpdater))]
//[CanEditMultipleObjects]
//public class TraitBadgeUpdaterEditor: Editor
//{
//
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        
//        
//        TraitBadgeUpdater myScript = (TraitBadgeUpdater) target;
//        
//        Dictionary<Food, string> spriteNames = new Dictionary<Food, string>()
//        {
//            {Food.Biomass, "org"},
//            {Food.Metal, "met"},
//            {Food.Rock, "rock"},
//            {Food.Water, "water"},
//            {Food.Ice, "ice"},
//            {Food.Fire, "fire"},
//            {Food.Electric, "electro"},
//            {Food.Radioactive, "radio"},
//        };
//
//        if (GUILayout.Button("Update Badge"))
//        {
//           
//            
//            myScript.gameObject.name = myScript.traitToRepresent.name;
//            myScript.gameObject.transform.Find("NamePlate").GetChild(0).GetComponent<Text>().text = myScript.traitToRepresent.name;
//
//            myScript.gameObject.transform.Find("Text").GetComponent<Text>().text =
//                $"0/{myScript.traitToRepresent.activationThreshold}";
//            
//            myScript.gameObject.transform.Find("Positive").GetChild(0).GetComponent<Image>().sprite =
//                (Sprite) AssetDatabase.LoadAssetAtPath($"Assets/Editor Default Resources/Icons/icon-{spriteNames[myScript.traitToRepresent.activationFood]}.png", typeof(Sprite));
//
//            Debug.Log($"Icons/icon-{spriteNames[myScript.traitToRepresent.activationFood]}.png");
//
//            if (myScript.traitToRepresent.detrimentalFood != Food.None)
//            {
//                myScript.gameObject.transform.Find("Negative").gameObject.SetActive(true);
//                myScript.gameObject.transform.Find("Negative").GetChild(0).GetComponent<Image>().sprite =
//                    (Sprite) AssetDatabase.LoadAssetAtPath(
//                        $"Assets/Editor Default Resources/Icons/icon-{spriteNames[myScript.traitToRepresent.detrimentalFood]}.png",
//                        typeof(Sprite));
//                
//                Debug.Log($"Icons/icon-{spriteNames[myScript.traitToRepresent.detrimentalFood]}.png");
//            }
//
//            else
//            {
//                myScript.gameObject.transform.Find("Negative").gameObject.SetActive(false);
//            }
//        }
//    }
//}