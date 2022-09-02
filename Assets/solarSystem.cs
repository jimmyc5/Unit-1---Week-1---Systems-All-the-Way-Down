using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solarSystem : MonoBehaviour
{
    public bool sorted = false;
    private bool isSorted = false;
    public List<GameObject> planets = new List<GameObject>();
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
                originalLocations.Add(childPlanet.gameObject, new Vector3(childPlanet.localPosition.x, childPlanet.localPosition.y, childPlanet.localPosition.z));
            }
            originalSpeeds.Add(childOrbit.gameObject, childOrbit.gameObject.GetComponent<ObjectRotator>().orbitSpeed);
        }
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
