using UnityEngine;
using System.Collections;

public class NpcPath {

    private Vector3 lastDestination;
    private Vector3[] path;
    private int curPathIndex;
    private bool isFinalPoint;

    private MonoBehaviour npc;

    public MonoBehaviour NPC { get { return npc; } }

    public NpcPath(MonoBehaviour npc)
    {
        this.npc = npc;
    }

    public void SetLastDestination(Vector3 position)
    {
        lastDestination = position;
    }

    public bool HasDestinationChanged(Vector3 destination)
    {
        var deltaDestination = destination - lastDestination;
        return deltaDestination.sqrMagnitude > 0.1f;
    }

    public void SetDestination(Vector3 destination)
    {
        var navPath = new NavMeshPath();
        if (NavMesh.CalculatePath(NPC.transform.position, destination, NavMesh.AllAreas, navPath))
        {
            path = navPath.corners;
            curPathIndex = 0;
        }
        else
        {
            path = new[] { NPC.transform.position };
            curPathIndex = 0;
        }
    }

    public void Update()
    {
        var toCurPathPos = AtHeight(path[curPathIndex], 0f) - AtHeight(NPC.transform.position, 0f);
        if (toCurPathPos.sqrMagnitude < 0.1f)
        {
            curPathIndex++;
            isFinalPoint = curPathIndex >= path.Length - 1;
        }

        // Arrive and avoid out of range indexes.
        if (curPathIndex > path.Length - 1)
        {
            Debug.Log("DESTINATION REACHED!");
            curPathIndex = path.Length - 1;
        }
    }

    public bool IsFinalPathPoint()
    {
        return isFinalPoint;
    }

    public Vector3 GetCurrentPathTargetPosition()
    {
        return path[curPathIndex];
    }

    private Vector3 AtHeight(Vector3 postion, float height)
    {
        return new Vector3(postion.x, height, postion.z);
    }
}
