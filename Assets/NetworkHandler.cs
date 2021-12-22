using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Siccity.GLTFUtility;
using Newtonsoft.Json;

[System.Serializable]
public class NetworkHandler : MonoBehaviour
{
    GameObject wrapper;
    public string filePath = "Assets/Files/";
    public string cloudURL;
    public string jsonData;
    public object objectData;

    public List<Component> components;
    public ObjectBuilder objectBuilder;

    private void Start()
    {
        jsonData = File.ReadAllText("Assets/request.json");
        setJSON(jsonData);

        objectBuilder = gameObject.GetComponent<ObjectBuilder>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Init();
        }
    }  

    public void Init(){
        foreach (Component component in components) {
            Debug.Log(component.name);
            Debug.Log(component.id);
            Debug.Log(component.url);

            DownloadFile(component);
        }
    }
    
    public void setJSON(string json){
        components = JsonConvert.DeserializeObject<List<Component>>(json); 
    }

    public void DownloadFile(Component component)
    {
        string path = GetFilePath(component.url);
        component.path = path;

        if (File.Exists(path))
        {
            Debug.Log($"{component.name} Found file locally, loading...");

            objectBuilder.Build(component);
            return;
        } 

        StartCoroutine(GetFileRequest(component.url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                // Log any errors that may happen
                Debug.Log($"{req.error} : {req.downloadHandler.text}");
            } else
            {
                Debug.Log($"{component.name} Downloaded");

                objectBuilder.Build(component);
            }
        }));
    }

    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length - 1];

        return $"{filePath}{filename}";
    }

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using(UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));
            yield return req.SendWebRequest();

            callback(req);
        }
    }

}
