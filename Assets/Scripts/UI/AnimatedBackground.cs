using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    [SerializeField] private Image backGround;
    [SerializeField] private float changeTimer = 1f;

    private int _currentImage = 0;
    private float _currentTimer = 0f;

    private void Start()
    {
        if(images.Length > 0)
            backGround.sprite = images[_currentImage];
    }

    private void Update()
    {
        if (images.Length <= 0)
            return;
        
        _currentTimer -= Time.deltaTime;
        if (_currentTimer <= 0)
        {
            _currentImage = (_currentImage + 1) % images.Length;
            backGround.sprite = images[_currentImage];
            _currentTimer = changeTimer;
        }
    }
}
