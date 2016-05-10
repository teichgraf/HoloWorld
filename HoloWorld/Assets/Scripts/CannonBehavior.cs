﻿using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.VR.WSA.Input;

public class CannonBehavior : MonoBehaviour
{
    private GestureRecognizer _gestureRecognizer;

    public float ForceMagnitude = 300f;
    public GameObject GazeCursor;
    public Material CannonMaterial;

    void Start()
    {
        _gestureRecognizer = new GestureRecognizer();
        _gestureRecognizer.TappedEvent += GestureRecognizerOnTappedEvent;
        _gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        _gestureRecognizer.StartCapturingGestures();
    }

    private void GestureRecognizerOnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        var eyeball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        eyeball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        eyeball.GetComponent<Renderer>().material = CannonMaterial;

        var rigidBody = eyeball.AddComponent<Rigidbody>();
        rigidBody.mass = 0.5f;
        rigidBody.position = transform.position;
        rigidBody.AddForce(transform.forward * ForceMagnitude);
    }
    
    void Update()
    {
        if (GazeCursor == null) return;

        var raycastHits = Physics.RaycastAll(transform.position, transform.forward);
        var firstHit = raycastHits.OrderBy(r => r.distance).FirstOrDefault();

        GazeCursor.transform.position = firstHit.point;
        GazeCursor.transform.forward = firstHit.normal;
    }

}