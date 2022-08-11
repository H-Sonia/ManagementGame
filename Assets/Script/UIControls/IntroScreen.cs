using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{

    public TMP_Text textField;
    public Image imageField;

    [TextArea(3,10)]
    public string[] textToDisplay;
    int textIndex = -1;

    public Sprite[] images;
    int imageindex = 0;

    [SerializeField]
    bool isEnding = false;

    [SerializeField]
    bool keepimage = false;

    // Start is called before the first frame update
    void Start()
    {
        textField.text = textToDisplay[0];
        imageField.sprite = images[0];
        if(isEnding)
            textField.canvasRenderer.SetAlpha(0);
    }

    public void NextButton()
    {
        if (!keepimage)
        {
            if (imageindex == 1)
                imageField.transform.gameObject.SetActive(false);
            if (imageindex < 1)
            {
                imageindex++;
                imageField.sprite = images[imageindex];

            }
            else if (textIndex < textToDisplay.Length - 1)
            {
                textIndex++;
                textField.text = textToDisplay[textIndex];
                //Debug.Log(textIndex);
            }
            else
            {
                print(isEnding);
                if (!isEnding)
                    SceneManager.LoadScene("UiTest");
                else
                    SceneManager.LoadScene(0);
            }
        }
        else
        {
            if(textIndex == -1)
            {
                textIndex++;
                
                imageField.CrossFadeColor(new Color(0.349f, 0.349f, 0.349f),5,false, false);

                //textField.color = new Color(1f, 1f, 1f, 0);

                textField.CrossFadeAlpha(1f, 5, false);
            }
            else if (textIndex < textToDisplay.Length - 1)
            {
                textIndex++;
                textField.text = textToDisplay[textIndex];
            }
            else
            {
                print(isEnding);
                if (!isEnding)
                    SceneManager.LoadScene("UiTest");
                else
                    SceneManager.LoadScene(0);
            }
        }
    }
}
