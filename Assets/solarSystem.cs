using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solarSystem : MonoBehaviour
{
    // J- preferable to use [SerializeField] w/ private vs public field 
    //public bool sorted = false;
    [SerializeField] bool sorted = false;
    private bool isSorted = false;

    // J- planets might not need to be public?
    List<GameObject> planets = new List<GameObject>();
    Dictionary<GameObject, Vector3> originalLocations = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, float> originalSpeeds = new Dictionary<GameObject, float>();
    private float maxDist = 15;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform childOrbit in transform)
        {
            foreach(Transform childPlanet in childOrbit)
            {
                planets.Add(childPlanet.gameObject);

                //J - no need to create a new Vector, just grab localPosition in total
                //originalLocations.Add(childPlanet.gameObject, new Vector3(childPlanet.localPosition.x, childPlanet.localPosition.y, childPlanet.localPosition.z));
                originalLocations.Add(childPlanet.gameObject, childPlanet.localPosition);
            }
            //J - put long references on a new line for readability or
            //(better) create a variable
            originalSpeeds.Add(childOrbit.gameObject, 
                childOrbit.gameObject.GetComponent<ObjectRotator>().orbitSpeed);
            //var originalOrbitSpeed = childOrbit.gameObject.GetComponent<ObjectRotator>().orbitSpeed;
            //originalSpeeds.Add(childOrbit.gameObject, originalOrbitSpeed);

        }
        //J - OOOOoooooo I love this, as this is the kind of thing I SUCK at,
        //so I'm thrilled to see elegant ways to do stuff that I would write spaghetti
        //to accomplish
        planets.Sort(delegate (GameObject x, GameObject y)
        {
            return x.transform.localScale.z.CompareTo(y.transform.localScale.z);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(sorted && !isSorted)
        {
            setToSorted();
            isSorted = true;
        }else if(!sorted && isSorted)
        {
            setToOriginal();
            isSorted = false;
        }
    }

    void setToSorted()
    {
        for(int i=0;i<planets.Count;i++)
        {
            planets[i].transform.localPosition = new Vector3(0,0,1.4f+ maxDist * ((float)i / (float)planets.Count));
            planets[i].transform.parent.GetComponent<ObjectRotator>().orbitSpeed = 0.01f * (maxDist * ((float)i / (float)planets.Count));
        }
    }

    void setToOriginal()
    {
        foreach(GameObject planet in planets)
        {
            planet.transform.localPosition = originalLocations[planet];
            planet.transform.parent.GetComponent<ObjectRotator>().orbitSpeed = originalSpeeds[planet.transform.parent.gameObject];
        }
    }
}
