using System.Collections;
using Mapbox.Unity.Map;
using UnityEngine;

public class LocationService : MonoBehaviour
{
    [SerializeField]
    AbstractMap map;

    [Header("Testing")]
    [SerializeField]
    string placeholderLocation = "";
    

	IEnumerator StartLocationService()
    {
        Debug.Log("Location service started!");

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location is not enabled by user! Using placeholder location.");
            map.Options.locationOptions.latitudeLongitude = placeholderLocation;

            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed out.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location.");
            yield break;
        }

        else
        {
            map.Options.locationOptions.latitudeLongitude = string.Format("{0}, {1}", Input.location.lastData.longitude, Input.location.lastData.latitude);
        }
    }

    void Awake()
    {
        StartCoroutine(StartLocationService());
    }
}
