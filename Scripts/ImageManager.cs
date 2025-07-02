using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ImageManager : MonoBehaviour
{
    private ARTrackedImageManager _trackedIM;
    public GameObject[] imagePrefabs;


    Dictionary<string, GameObject> _ARObjects = new Dictionary<string, GameObject>();

    void Start()
    {
        _trackedIM = GetComponent<ARTrackedImageManager>();
        _trackedIM.trackablesChanged.AddListener(OnTrackedImageChange);
        AR_Setup();
    }

    void OnDestroy()
    {
        _trackedIM.trackablesChanged.RemoveListener(OnTrackedImageChange);
    }

    void AR_Setup()
    {
        foreach (var prefab in imagePrefabs)
        {
            var arObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            arObj.name = prefab.name;
            arObj.gameObject.SetActive(false);
            _ARObjects.Add(arObj.name, arObj);
        }
    }


    void OnTrackedImageChange(ARTrackablesChangedEventArgs<ARTrackedImage> edit)
    {
        foreach (var item in edit.added)
        {
            
            _ARObjects[item.referenceImage.name].gameObject.SetActive(true);
        }

        foreach (var item in edit.updated)
        {
            _ARObjects[item.referenceImage.name].gameObject.SetActive(item.trackingState == TrackingState.Tracking);
            _ARObjects[item.referenceImage.name].transform.position = item.transform.position;
            _ARObjects[item.referenceImage.name].transform.rotation = item.transform.rotation;
        }
        
    }

    // void UpdateTrackedImage(ARTrackedImage tracked) -- Saved function for later
    // {
    //     if (tracked != null) return;

    //     if (tracked.trackingState is TrackingState.Limited or TrackingState.None)
    //     {
    //         _ARObjects[tracked.referenceImage.name].gameObject.SetActive(false);
    //         return;
    //     }
    //     _ARObjects[tracked.referenceImage.name].gameObject.SetActive(true);
    //     _ARObjects[tracked.referenceImage.name].gameObject.SetActive(tracked.trackingState == TrackingState.Tracking);
    //     _ARObjects[tracked.referenceImage.name].transform.position = tracked.transform.position;
    //     _ARObjects[tracked.referenceImage.name].transform.rotation = tracked.transform.rotation;
        
    // }

}
