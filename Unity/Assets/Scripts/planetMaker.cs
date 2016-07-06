using UnityEngine;
using System.Collections.Generic;

public class planetMaker : MonoBehaviour {

    public static planetMaker instance;
    public List<GameObject> planetPrefabs;

    //If current planets is less than max planets
    //% chance to spawn an planet

    //Pick a random point along the top of the screen to place the new planet
    //Pick a random planet

    //Sprite that moves down the screen, MAKE SURE IT'S BENEATH BULLETS AND ACTORS
    //When planet reaches point below screen, kill it and update planet count

    public int maxPlanets = 3;
    List<GameObject> planets = new List<GameObject>();
    public float MaxSpawnCoolDown = 4;
    float cooldown;

    void Start()
    {
        instance = this;
    }
    
    public void RemovePlanetFromList(GameObject go)
    {
        planets.Remove(go);
    }

    void SpawnPlanet()
    {
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(.4f,.6f), 1.2f));
        spawnPos.z = 5;
        GameObject newPlanet = Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Count)],spawnPos,transform.rotation) as GameObject;

        //Rotate planets random amounts
        float ran = Random.value,rot=0;
        if (ran < 0.25f)
            rot = 0;
        else if (ran < .5f)
            rot = 90;
        else if (ran < .75f)
            rot = 180;
        else if (ran < 1)
            rot = 270;
        newPlanet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));

        newPlanet.transform.parent = transform;
        planets.Add(newPlanet);
    }

    void Update()
    {
        if (planets.Count < maxPlanets && cooldown <= 0)
        {
            if (Random.value > 0.95f)
            {
                SpawnPlanet();
                cooldown = MaxSpawnCoolDown;
            }
        }
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
}
