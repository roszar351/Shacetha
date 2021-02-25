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
    public float moveYSpeed = 1f;

    public static TextPopup Create(Vector3 position, int damageAmount)
    {
        Transform textPopupTransform = Instantiate(GameAssets.I.pfTextPopup, position, Quaternion.identity);
        TextPopup textPopup = textPopupTransform.GetComponent<TextPopup>();
        textPopup.SetUp(damageAmount);

        return textPopup;
    }

    public static TextPopup Create(Vector3 position, int damageAmount, Color newColor)
    {
        Transform textPopupTransform = Instantiate(GameAssets.I.pfTextPopup, position, Quaternion.identity);
        TextPopup textPopup = textPopupTransform.GetComponent<TextPopup>();
        textPopup.SetUp(damageAmount, newColor);

        return textPopup;
    }

    public static TextPopup Create(Vector3 position, string message, Color newColor)
    {
        Transform textPopupTransform = Instantiate(GameAssets.I.pfTextPopup, position, Quaternion.identity);
        TextPopup textPopup = textPopupTransform.GetComponent<TextPopup>();
        textPopup.SetUp(message, newColor);

        return textPopup;
    }

    private TextMeshPro _popupText;
    private float _popupLifeTime = .5f;
    private Color _textColor;

    private void Awake()
    {
        _popupText = GetComponent<TextMeshPro>();
    }

    public void SetUp(int damageAmount)
    {
        _popupText.SetText(damageAmount.ToString());
    }

    public void SetUp(int damageAmount, Color newColor)
    {
        SetUp(damageAmount.ToString(), newColor);
    }

    public void SetUp(string message, Color newColor)
    {
        _popupText.SetText(message);
        _popupText.color = newColor;
        _textColor = _popupText.color;
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        _popupLifeTime -= Time.deltaTime;
        if (_popupLifeTime < 0)
        {
            float disappearSpeed = 3f;
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _popupText.color = _textColor;
            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
