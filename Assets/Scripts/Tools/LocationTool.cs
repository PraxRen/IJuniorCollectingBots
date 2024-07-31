using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTool : MonoBehaviour
{
    [SerializeField] private Tool _tool;
    [SerializeField] private TypeTool _type;

    public TypeTool Type => _type;
    public Tool Tool => _tool;
}
