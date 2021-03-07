using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Hold references to some commonly used prefabs
 */
public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets I
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    public Transform pfTextPopup;
    public Material diffuseMaterial1;
    public Material diffuseMaterial2;
}