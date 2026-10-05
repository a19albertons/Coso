using Unity.VisualScripting;
using UnityEngine;

public class PlayerPositionSpawn : MonoBehaviour
{
    private Vector3 puntoSpawn;
    void Awake()
    {
        puntoSpawn = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
    }

    


    public void SpawnPlayer()
    {
        transform.position = puntoSpawn;
        gameObject.SetActive(true);
    }
}