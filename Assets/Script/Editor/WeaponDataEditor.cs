using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using System;
using UnityEngine;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    WeaponData weaponData;
    string[] weaponSubtypes;
    int selectedWeaponSubtype;

    void OnEnable() // cai thien load game
    {
        // Cache the weapon data value.
        weaponData = (WeaponData)target;

        // Retrieve all the weapon subtypes and cache it.
        System.Type baseType = typeof(Weapon);
        List<System.Type> subTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList();
        
        //Add a none option in front
         List<string> subTypesString = subTypes.Select(t => t.Name).ToList();
        subTypesString.Insert(0, "None");
        weaponSubtypes = subTypesString.ToArray();
       

        // Ensure that we are using the correct weapon subtype
        selectedWeaponSubtype = Math.Max(0,Array.IndexOf(weaponSubtypes, weaponData.behaviour));
    }
    public override void OnInspectorGUI()
    {


        // draw a dropdown in the Inspector
        selectedWeaponSubtype = EditorGUILayout.Popup("Behaviour",Math.Max(0, selectedWeaponSubtype), weaponSubtypes);

        if(selectedWeaponSubtype > -1)
        {
            // upsdates the behaviour 
            weaponData.behaviour = weaponSubtypes[selectedWeaponSubtype].ToString();
            EditorUtility.SetDirty(weaponData); // mark the object to save
            DrawDefaultInspector(); // Draw the default inspector elements
          
        }
        
    }
}
