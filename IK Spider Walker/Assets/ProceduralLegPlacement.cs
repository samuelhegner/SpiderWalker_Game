using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLegPlacement : MonoBehaviour
{
    [SerializeField] private GameObject _rootParentGameObject;
    [SerializeField] private GameObject _ikTarget;

    [SerializeField] private LayerMask _layerMask;

    private Vector3 _localSpaceRaycastSpawn;

    private Vector3 _newLegPlacementTarget;
    private Vector3 _oldLegPlacementTarget;
    
    [SerializeField] private Transform _offsetRaycastSpawnPoint;

    [SerializeField] private float _maximumDistanceToNewPlacement;

    [SerializeField] private bool debug; 
    
    void Start()
    {
        _localSpaceRaycastSpawn = _offsetRaycastSpawnPoint.position;

        _oldLegPlacementTarget = _ikTarget.transform.position;
    }

    void Update()
    {
        _newLegPlacementTarget = RayCastForNewTarget();

        if (Vector3.Distance(_ikTarget.transform.position, _newLegPlacementTarget) > _maximumDistanceToNewPlacement)
        {
            _oldLegPlacementTarget = _newLegPlacementTarget;
        }

        _ikTarget.transform.position = _oldLegPlacementTarget;
        
        UpdateRaycastSpawnPoint();
    }


    void UpdateRaycastSpawnPoint()
    {
        _localSpaceRaycastSpawn = _offsetRaycastSpawnPoint.position + _rootParentGameObject.transform.forward;
        
    }

    private Vector3 RayCastForNewTarget()
    {
        Ray ray = new Ray(_localSpaceRaycastSpawn, -_rootParentGameObject.transform.up);
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * 3f);

        if (Physics.Raycast(ray, out hit, 3f, _layerMask))
        {
            return hit.point;
        }
        else
        {
            return _oldLegPlacementTarget;
        }
    }


    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_localSpaceRaycastSpawn, 0.05f); //draw raycast spawn point
            
            Gizmos.DrawSphere(_newLegPlacementTarget, 0.05f); //draw new leg placement point
        }
    }
}
