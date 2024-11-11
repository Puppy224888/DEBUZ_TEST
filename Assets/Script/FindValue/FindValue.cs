using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FindValue : MonoBehaviour
{
    [SerializeReference] TMP_InputField x_Value;
    [SerializeReference] TMP_InputField y_Value;
    [SerializeReference] TMP_InputField position_Value;
    [SerializeReference] TextMeshProUGUI results;
    [SerializeReference] Button btn;
    public void FindNthDecimal()
    {
        Debug.Log("Run");
        if (!string.IsNullOrEmpty(x_Value.text) && !string.IsNullOrEmpty(y_Value.text) && !string.IsNullOrEmpty(position_Value.text))
        {
            Debug.Log("RunGo");
            var x = int.Parse(x_Value.text);
            var y = int.Parse(y_Value.text);
            var p = int.Parse(position_Value.text);

            int remainder = x % y;
            Debug.Log($"remainder : {remainder}");
            for (int i = 1; i < p; i++)
            {
                remainder *= 10;
                remainder %= y;
                Debug.Log($">>>>>>>{remainder}");
            }
            remainder *= 10;
            results.text = $"{remainder / y}";
        }
    }
}
