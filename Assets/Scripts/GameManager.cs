using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        SpawnCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnPlayer() 
    {
       Player = Instantiate(PlayerPrefab, new Vector3(0, 3, 0), Quaternion.identity);
    }
    private void SpawnCamera() 
    {
        Instantiate(CameraPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
