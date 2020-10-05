using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Creates a small ui popup in desired position,
 * Usually used for damage values but can be used
 * to display text.
 * Allows to also supply color of text
 * 
 */
public class TextPopup : MonoBehaviour
{
    public static TextPopup Create(Vector3 position, int damageAmount)
    {
        Transform textPopupTransform = Instantiate(GameAssets.i.pfTextPopup, position, Quaternion.identity);
        TextPopup textPopup = textPopupTransform.GetComponent<TextPopup>();
        textPopup.SetUp(damageAmount);

        return textPopup;
    }

    public static TextPopup Create(Vector3 position, int damageAmount, Color newColor)
    {
        Transform textPopupTransform = Instantiate(GameAssets.i.pfTextPopup, position, Quaternion.identity);
        TextPopup textPopup = textPopupTransform.GetComponent<TextPopup>();
        textPopup.SetUp(damageAmount, newColor);

        return textPopup;
    }

    public static TextPopup Create(Vector3 position, string message, Color newColor)
    {
        Transform textPopupTransform = Instantiate(GameAssets.i.pfTextPopup, position, Quaternion.identity);
        TextPopup textPopup = textPopupTransform.GetComponent<TextPopup>();
        textPopup.SetUp(message, newColor);

        return textPopup;
    }

    private TextMeshPro popupText;
    private float popupLifeTime = .5f;
    private Color textColor;

    private void Awake()
    {
        popupText = GetComponent<TextMeshPro>();
    }

    public void SetUp(int damageAmount)
    {
        popupText.SetText(damageAmount.ToString());
    }

    public void SetUp(int damageAmount, Color newColor)
    {
        SetUp(damageAmount.ToString(), newColor);
    }

    public void SetUp(string message, Color newColor)
    {
        popupText.SetText(message);
        popupText.color = newColor;
        textColor = popupText.color;
    }

    private void Update()
    {
        float moveYSpeed = 3f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        popupLifeTime -= Time.deltaTime;
        if (popupLifeTime < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            popupText.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
