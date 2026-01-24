using UnityEngine;
using System.Collections.Generic; 

public class DogController : MonoBehaviour
{
    public GameObject followTarget;
    public float distance = 1.3f; 
    public float speed = 0.03f; 
    public GameObject DogPrefab; 
    public int initialDogCount = 3;
    
    private List<GameObject> activeDogs = new List<GameObject>();

    void Start()
    {   
        spawnDogs(initialDogCount);
    }

    void spawnDogs(int count)
    {
        for (int i = 0; i < count; i++) 
        {
            GameObject newDog = Instantiate(DogPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Dog dogScript = newDog.GetComponent<Dog>();
            dogScript.Init(distance, speed, followTarget);
            activeDogs.Add(newDog);
        }
    }

    void Update()
    {
        
    }

    public void removeDog()
    {
        if (activeDogs.Count > 0)
        {
            GameObject dogToRemove = activeDogs[activeDogs.Count - 1];
            activeDogs.RemoveAt(activeDogs.Count - 1);
            Destroy(dogToRemove);
        }
    }

    public void resetDog()
    {
        foreach (GameObject dog in activeDogs)
        {
            if(dog != null) Destroy(dog);
        }
        activeDogs.Clear();
        spawnDogs(initialDogCount);
    }
}