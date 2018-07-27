using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ParallaxLayerData
{
    public float xMultiplier, yMultiplier;
    public float width, height;
    public Transform layerObject;
}

public class ParallaxHandler : MonoBehaviour
{
    Camera camera;

    Vector3 cameraLastPos;
    Vector2 camVelocity;

    GameObject y;
    GameObject x;

    public ParallaxLayerData[] layers;


    [ContextMenu("Auto Set Layers")]
    public void SetLayers()
    {
        ParallaxLayerData[] layers = new ParallaxLayerData[transform.childCount];

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].layerObject = transform.GetChild(i);

            Vector2 center = layers[i].layerObject.position;
            float xMin = center.x, xMax = center.x, yMin = center.y, yMax = center.y;

            foreach (SpriteRenderer r in layers[i].layerObject.GetComponentsInChildren<SpriteRenderer>())
            {
                xMin = r.bounds.min.x < xMin ? r.bounds.min.x : xMin;
                xMax = r.bounds.max.x > xMax ? r.bounds.max.x : xMax;
                yMin = r.bounds.min.y < yMin ? r.bounds.min.y : yMin;
                yMax = r.bounds.max.y > yMax ? r.bounds.max.y : yMax;
            }

            layers[i].width = xMax - xMin;
            layers[i].height = yMax - yMin;
            
            
            //Previously set multiplier is retained, to avoid having to set new each time you change width/height
            if (this.layers.Length > i - 1)
            {
                layers[i].xMultiplier = this.layers[i].xMultiplier;
                layers[i].yMultiplier = this.layers[i].yMultiplier;
            }

        }

        this.layers = layers;
    }

    void Start()
    {
        camera = Camera.main;
    }

    void LateUpdate()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        camVelocity = camera.transform.position - cameraLastPos;

        cameraLastPos = camera.transform.position;

        for (int i = 0; i < layers.Length; i++)
        {
            Transform layer = layers[i].layerObject;

            layer.transform.position += (Vector3)(camVelocity * new Vector2(layers[i].xMultiplier, layers[i].yMultiplier));

            if (Mathf.Abs(layer.transform.position.x - camera.transform.position.x) > layers[i].width/4f)
            {
                layer.transform.position += Vector3.right * Mathf.Sign(camera.transform.position.x - layer.transform.position.x) * layers[i].width / 2f;
            }

            /* Vertical Tiling, not used at the moment
            if (Mathf.Abs(transform.position.y - camera.transform.position.y) > 1000f)
            {
                transform.position += Vector3.up * Mathf.Sign(camera.transform.position.y - transform.position.y) * 30;
            }
            */
        }
    }
}
