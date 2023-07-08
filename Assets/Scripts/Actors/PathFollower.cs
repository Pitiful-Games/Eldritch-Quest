using System;
using PathCreation;
using UnityEngine;

public class PathFollower : MonoBehaviour {
    public delegate void OnPathEnd(PathFollower pathFollower);
    
    public event OnPathEnd PathEnded;

    public float tweenTime = 1;
    public PathCreator pathCreator;
    public float travelSpeed = 3;
    
    private float distanceTraveled;

    private void Update() {
        if (!pathCreator) return;

        distanceTraveled += travelSpeed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTraveled);
        if (distanceTraveled >= pathCreator.path.length) {
            distanceTraveled = 0;
            PathEnded?.Invoke(this);
        }
    }
    
    public void ChangePath(PathCreator newPath) {
        var targetPosition = newPath.path.GetPointAtDistance(distanceTraveled);
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", targetPosition,
            "easetype", iTween.EaseType.linear,
            "time", tweenTime,
            "oncomplete", "OnPathChangeComplete",
            "oncompleteparams", newPath
        ));
    }

    private void OnPathChangeComplete(PathCreator newPath) {
        pathCreator = newPath;
        distanceTraveled = newPath.path.GetClosestDistanceAlongPath(transform.position);
    }
}
