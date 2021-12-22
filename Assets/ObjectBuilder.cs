using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Siccity.GLTFUtility;
using Newtonsoft.Json;

[System.Serializable]
public class ObjectBuilder : MonoBehaviour
{
    GameObject wrapper;
    public bool building = false;
    List<Component> queueOfComponents;
    // Start is called before the first frame update
    void Start()
    {
        queueOfComponents = new List<Component>();
        wrapper = new GameObject();
        wrapper.name = "Wrapper";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build(Component component)
    {
        if(building){
            queueOfComponents.Add(component);
        }else{
            
            building = true;

            GameObject model = Importer.LoadFromFile(component.path);

            building = false;

            model.AddComponent<Renderer>();

            model.name = component.name;
            model.transform.SetParent(wrapper.transform);

       

            if(component.material != null){
                if(component.material.type == "color"){
                    for(int i = 0; i < model.transform.childCount; i++)
                    {
                        Transform child =  model.transform.GetChild(i);
                        child.gameObject.GetComponent<Renderer>().material.color = HexToColor(component.material.source);
                    }
                }else if(component.material.type == "image"){

                }
            }

            if(component.size != null){
                Vector3 newScale = model.transform.localScale;
                foreach (Size sizeItem in component.size) {
                    switch(sizeItem.axis){
                        case "x":
                            newScale.x = sizeItem.value;
                            break;
                        case "y":
                            newScale.y = sizeItem.value;
                            break;
                        case "z":
                            newScale.z = sizeItem.value;
                            break;
                    }
                }
                model.transform.localScale = newScale;
            }

            if(component.selfReference != null && component.parentReference != null){
                model.transform.position = GameObject.Find(component.parentReference).transform.position - 
                                           model.transform.Find(component.selfReference).transform.position;
            }
          
            if(queueOfComponents.Count > 0){
                Component nextComponent = queueOfComponents[0];
                queueOfComponents.RemoveAt(0);
                Build(nextComponent);
            }
        }
    }


    Color HexToColor(string hexString)
    {
        //replace # occurences
        if (hexString.IndexOf('#') != -1)
            hexString = hexString.Replace("#", "");

        int r,g,b = 0;

        r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
        g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
        b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

        return new Color(r, g, b);
    }
    // void LoadModel(string path, string parent, string reference)
    // {
    //     model.transform.SetParent(wrapper.transform);

    //     if(reference != ""){
    //         Debug.Log(GameObject.Find(parent).transform.position);
    //         Debug.Log(GameObject.Find(reference).transform.position);

    //     }

    // }
}
