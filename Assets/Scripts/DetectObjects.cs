using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjects : MonoBehaviour
{
    HashSet<Rigidbody> _obstacles = new HashSet<Rigidbody>();

    public HashSet<Rigidbody> Obstacles
    {
        get
        {
            _obstacles.RemoveWhere(IsNull);
            return _obstacles;
        }
    }

    static bool IsNull(Rigidbody r)
    {
        return r == null || r.Equals(null);
    }

    void AddToRadar(Collider r)
    {
        Rigidbody rb = r.GetComponent<Rigidbody>();

        if (rb != null)
        {
            _obstacles.Add(rb);
            Debug.Log("RB added!");
        }
    }

    void RemoveFromRadar(Collider r)
    {
        Rigidbody rb = r.GetComponent<Rigidbody>();

        if (rb != null)
        {
            _obstacles.Remove(rb);
            Debug.Log("RB removed!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AddToRadar(other);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveFromRadar(other);
    }
}
