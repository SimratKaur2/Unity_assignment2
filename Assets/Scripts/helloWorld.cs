using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class helloWorld : MonoBehaviour
{
    public string firstName;
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            textMeshPro.text = $"Hello {firstName}!";
            Debug.Log($"Text set to: {textMeshPro.text}"); // Log the text that is set
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on the GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update is not used here but could be used for continuous checks or updates per frame
    }
}
