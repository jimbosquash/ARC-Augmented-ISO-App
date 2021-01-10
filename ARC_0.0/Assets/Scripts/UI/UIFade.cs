using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class UIFade : MonoBehaviour
{
    [SerializeField] [Range(0.005f, 0.1f)]private float speed = 0.016f;
    [SerializeField] [Range(0.00f, 1f)] private float capMin;
    [SerializeField] [Range(0.00f, 1)] private float capMax;
    private Image _image;
    private float _alpha;
    private Color _imageColor;
    public bool CapFade = false;
    [SerializeField] private bool direction;

    void Start()
    {
        _image = GetComponent<Image>();
        _alpha = _image.color.a;
        _imageColor = _image.color;
    }

    void Update()
    {
        if (_image.enabled)
        {
            _imageColor = _image.color;

            if (CapFade)
            {
                if (direction)
                    _imageColor.a += speed;
                else
                    _imageColor.a -= speed;

                if (_imageColor.a >= capMax)
                    direction = false;
                if (_imageColor.a <= capMin)
                    direction = true;
            }
            else
            {
                if (direction)
                    _imageColor.a += speed;
                else
                    _imageColor.a -= speed;

                if (_imageColor.a >= 1)
                    direction = false;
                if (_imageColor.a <= 0)
                    direction = true;
            }
            
            _image.color = _imageColor;
        }
    }
}
