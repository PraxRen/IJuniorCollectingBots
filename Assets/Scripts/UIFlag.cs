using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlag : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Image _image;
    [SerializeField] private Color _colorPositiveBuilding;
    [SerializeField] private Color _colorNegativeBuilding;


    private void Update()
    {
        if (_flag.CanBuilding)
            _image.color = _colorPositiveBuilding;
        else
            _image.color = _colorNegativeBuilding;
    }
}
