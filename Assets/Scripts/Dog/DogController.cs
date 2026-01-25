using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public GameObject followTarget;
    public float distance = 1.3f;
    public float speed = 0.03f;
    public GameObject[] DogPrefabs;

    private List<GameObject> activeDogs = new List<GameObject>();



    public void spawnDogs(int count)
    {
        if (count > DogPrefabs.Length)
        {
            count = DogPrefabs.Length;
            Debug.LogWarning("Cannot spawn more dogs than what we have prefabs for!");
        }
        Debug.Log(count);
        for (int i = 0; i < count; i++)
        {
            GameObject newDog = Instantiate(DogPrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
            Dog dogScript = newDog.GetComponent<Dog>();
            dogScript.Init(distance, speed, followTarget);
            activeDogs.Add(newDog);
        }
    }

    public IEnumerator DelayDogErasure()
    {
        yield return new WaitForSeconds(0.3f);
        removeDog();
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

    // do we even need this
    public void resetDog()
    {
        foreach (GameObject dog in activeDogs)
        {
            if (dog != null) Destroy(dog);
        }
        activeDogs.Clear();
    }
}