using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randColorCS : MonoBehaviour
{

    struct Cube
    {
        public Vector3 position;
        public Color color;
    }

    public ComputeShader computeShader;
    public int interactions = 50;
    public int count = 100;
    GameObject[] gameObjects;
    Cube[] data;
    public GameObject modelPref;

 
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (data == null)
        {

            if (GUI.Button(new Rect(0, 0, 100, 50), "Create"))
            {
                createCube();
            }

        }

        if (data != null)
        {
            if (GUI.Button(new Rect(110, 0, 100, 50), "Random CPU"))
            {
                for (int k = 0; k < interactions; k++) {

                    for (int i = 0; i < gameObjects.Length; i++)
                    {
                        gameObjects[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
                    }
                }
            }
        }

        if (data != null)
        {
            if (GUI.Button(new Rect(220, 1, 100, 50), "Random GPU"))
            {
                int totalSize = 4 * sizeof(float) + 3 * sizeof(float);

                ComputeBuffer computeBuffer = new ComputeBuffer(data.Length, totalSize);
                computeBuffer.SetData(data);

                computeShader.SetBuffer(0, "cubes", computeBuffer);
                computeShader.SetInt("interaction", interactions);

                computeShader.Dispatch(0, data.Length / 10, 1, 1);

                computeBuffer.GetData(data);

                for (int i = 0; i < gameObjects.Length; i++)
                {
                    gameObjects[i].GetComponent<MeshRenderer>().material.SetColor("_Color", data[i].color);
                }

                computeBuffer.Dispose();
            }
        }
    }

    private void createCube()
    {
        data = new Cube[count * count];
        gameObjects = new GameObject[count * count];

        for (int i = 0; i < count; i++)
        {
            float offsetX = (-count / 2 + i);

            for (int j = 0; j < count; j++)
            {
                float offsetY = (-count / 2 + j);

                Color _color = Random.ColorHSV();

                GameObject go = GameObject.Instantiate(modelPref, new Vector3(offsetX * 1.2f, 0, offsetY * 1.2f), Quaternion.identity);
                go.GetComponent<MeshRenderer>().material.SetColor("_Color", _color);

                gameObjects[i * count + j] = go;

                data[i * count + j] = new Cube();
                data[i * count + j].position = go.transform.position;
                data[i * count + j].color = _color;
            }
        }
    }

    public void colorRandomizer(GameObject cube)
    {
        //Usar para trocar somente a cor do cubo passado
        int totalSize = 4 * sizeof(float) + 3 * sizeof(float);

        ComputeBuffer computeBuffer = new ComputeBuffer(data.Length, totalSize);
        computeBuffer.SetData(data);

        computeShader.SetBuffer(0, "cubes", computeBuffer);
        computeShader.SetInt("interaction", interactions);

        computeShader.Dispatch(0, data.Length / 10, 1, 1);

        computeBuffer.GetData(data);

        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", data[0].color);
        

        computeBuffer.Dispose();
    }
}
