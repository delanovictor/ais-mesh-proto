using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentMaterial
{
    public string type { get; set; }
    public string source { get; set; }
    public string reflection { get; set; }
}

[System.Serializable]
public class Size
{
    public string axis { get; set; }
    public float value { get; set; }
    public string variable { get; set; }
}

[System.Serializable]
public class Component
{
    public string id { get; set; }
    public string name { get; set; }
    public string url { get; set; }
    public string path { get; set; }
    public string selfReference { get; set; }
    public string parentReference { get; set; }
    public ComponentMaterial material { get; set; }
    public List<Size> size { get; set; }
}

