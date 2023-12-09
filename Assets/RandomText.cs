using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RandomText : MonoBehaviour
{
    
    // Start is called before the first frame update
    private static string[] adjectives = { "Swift", "Mighty", "Cunning", "Brave", "Shadow", "Invisible" };
    private static string[] nouns = { "Warrior", "Mage", "Rogue", "Paladin", "Hunter", "Wizard" };
    private static readonly System.Random random = new System.Random();
    private static List<string> prefixes = new List<string> { "Mystic", "Ancient", "Hidden", "Secret", "Forgotten", "Enchanted" };
    private static List<string> placeTypes = new List<string> { "Chamber", "Hall", "Grove", "Dungeon", "Sanctuary", "Library" };

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void GenerateNickname(TMP_InputField obj)
    {
        string adjective = adjectives[random.Next(adjectives.Length)];
        string noun = nouns[random.Next(nouns.Length)];
        obj.text =  $"{adjective} {noun}";
    }

    public static void GeneratePlaceName(TMP_InputField obj)
    {
        string prefix = prefixes[random.Next(prefixes.Count)];
        string placeType = placeTypes[random.Next(placeTypes.Count)];
        obj.text =  $"{prefix} {placeType}";
    }
}
