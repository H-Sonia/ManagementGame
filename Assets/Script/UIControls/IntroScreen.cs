using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    public TMP_Text textField;

    [TextArea(3,10)]
    public string[] textToDisplay;
    int textIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        textField.text = textToDisplay[0];
    }

    public void NextButton()
    {
        if(textIndex < textToDisplay.Length - 1)
        {
            textIndex++;
            textField.text = textToDisplay[textIndex];
            //Debug.Log(textIndex);
        }
        else
        {
            SceneManager.LoadScene("UiTest");
        }
    }
}
