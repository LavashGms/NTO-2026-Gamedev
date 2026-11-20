using UnityEngine;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour
{
    public List<GameObject> clientPrefabs;
    public List<double> clientSpawnChances;
    [SerializeField] private Vector3 clientsSpawnCoordinates;
    public Client GetClient()
    {
        Client newClient = Instantiate(clientPrefabs.Randomize(clientSpawnChances)).GetComponent<Client>();
        newClient.transform.position = clientsSpawnCoordinates;
        return newClient;
    }
}