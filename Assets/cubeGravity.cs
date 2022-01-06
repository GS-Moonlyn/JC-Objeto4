using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeGravity : MonoBehaviour
{
    public float minMass;
    public float maxMass;
    [SerializeField]
    float mass;
    public float minSpeed;
    public float maxSpeed;
    float curSpeed;
    [SerializeField]
    float iniSpeed;
    float force = 10;
    float t0;
    float dt;
    Vector3 iniPosition;
    bool isColliding = false;
    float floorPosition;
    float cubeMiddle;
    randColorCS rand;

    // Start is called before the first frame update
    void Start()
    {
        iniPosition = this.transform.position;
        mass = Random.Range(minMass, maxMass);
        //this.GetComponent<Rigidbody>().mass = Random.Range(minMass, maxMass);
        this.iniSpeed = Random.Range(minSpeed, maxSpeed);
        curSpeed = iniSpeed;
        t0 = Time.realtimeSinceStartup;
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        rand = GameObject.FindGameObjectWithTag("colorManager").GetComponent<randColorCS>();
        floorPosition = floor.GetComponent<MeshFilter>().mesh.bounds.extents.y * floor.transform.localScale.y + floor.transform.position.y;
        cubeMiddle = this.GetComponent<MeshFilter>().mesh.bounds.extents.y * this.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding == false)
        {
            dt = Time.realtimeSinceStartup - t0;
            curSpeed = iniSpeed + ((force / mass) * dt);
            float newPosition = -curSpeed * dt;
            if(newPosition < floorPosition + cubeMiddle)
            {
                newPosition = floorPosition + cubeMiddle;
                isColliding = true;
                rand.colorRandomizer(this.gameObject);

            }
            this.transform.position = iniPosition + new Vector3(0, newPosition);

        }
    }
}
