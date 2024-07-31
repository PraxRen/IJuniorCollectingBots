using TMPro;
using UnityEngine;

public class UIStorage : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _storage.Changed += OnChanged;
    }

    private void OnDisable()
    {
        _storage.Changed -= OnChanged;
    }

    private void OnChanged()
    {
        _text.text = _storage.Count.ToString();
    }
}
