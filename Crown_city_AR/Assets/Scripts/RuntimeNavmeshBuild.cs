using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Used to bake navmesh at runtime
public class RuntimeNavmeshBuild : MonoBehaviour
{
    [Header("Build on")]
    public GameObject Platform;


    private NavMeshSurface _navMeshSurface;
    // Start is called before the first frame update
    void Awake()
    {
        _navMeshSurface = GetComponent<NavMeshSurface>();

    }
    
    public void BuildMesh()
    {
        _navMeshSurface.BuildNavMesh(); // baked
    }
}
