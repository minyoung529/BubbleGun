using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapObject : MonoBehaviour
{
    new private Renderer renderer;
    [SerializeField] private Material tempMaterial;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        
        EventManager.StartListening("Win", ChangeMapColor);
    }

    private void ChangeMapColor()
    {
        List<Material> materials = new List<Material>(renderer.materials);
        materials.Insert(0, new Material(tempMaterial));

        renderer.materials = materials.ToArray();
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Win", ChangeMapColor);
    }
}
