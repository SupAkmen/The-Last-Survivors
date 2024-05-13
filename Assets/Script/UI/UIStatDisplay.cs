using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Reflection;
using UnityEngine;
using System.Text;

public class UIStatDisplay : MonoBehaviour
{
    public PlayerStats player; // The player that this stat display is rendering stats for
    public bool updateInEditor;
    TextMeshProUGUI statNames, statValues;

    void OnEnable()
    {
        UpdateStatField();
    }

    private void OnDrawGizmosSelected()
    {
        if (updateInEditor) UpdateStatField();
    }
    public void UpdateStatField()
    {
        if (player == null) return;
        // Get reference to both Text object to render stat name ang value
        if (!statNames) statNames = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (!statValues) statValues = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        // Render all stat names
        // use stringbuilder so that the string manipulation runs faster.
        StringBuilder names = new StringBuilder();
        StringBuilder values = new StringBuilder();

        FieldInfo[] fields = typeof(CharacterData.Stats).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            // Render stat name
            names.AppendLine(field.Name);

            // Get the stat value

            object val = field.GetValue(player.Stats);
            float fval = val is int ? (int)val : (float)val;

            // print it as a petcentage if irt has an attribute assigned anf is a float
            PropertyAttribute attribute = (PropertyAttribute)PropertyAttribute.GetCustomAttribute(field, typeof(PropertyAttribute));
            if (attribute != null && field.FieldType == typeof(float))
            {
                float percentage = Mathf.Round(fval * 100 - 100);
                // if the stat value is 0, hust put a dash
                if (Mathf.Approximately(percentage, 0))
                {
                    values.Append('-').Append('\n');
                }
                else
                {
                    if (percentage > 0)
                        values.Append('+');
                    values.Append(percentage).Append('%').Append("\n");
                }
            }
            else
            {
                values.Append(fval).Append("\n");
            }






            // Updates the fields with the strings we built
            statNames.text = PrettifyNames(names);
            statValues.text = values.ToString();
        }
    }
    public static string PrettifyNames(StringBuilder input)
    {
        if (input.Length <= 0) return string.Empty;

        StringBuilder result = new StringBuilder();
        char last = '\0';

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            // check when to uppercase or add spaces to a character
            if (last == '\0' || char.IsWhiteSpace(last))
            {
                c = char.ToUpper(c);
            }
            else if (char.IsUpper(c))
            {
                result.Append(' '); // Insert space before capital letter
            }
            result.Append(c);
            last = c;
        }

        return result.ToString();
    }

    private void Reset()
    {
        player = FindObjectOfType<PlayerStats>();
    }
}
