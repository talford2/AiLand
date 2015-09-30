using UnityEngine;

public class NpcPath {

    private Vector3 lastDestination;
    private Vector3[] path;
    private int curPathIndex;
    private bool isFinalPoint;
    private bool hasArrived;

    private MonoBehaviour npc;

    public MonoBehaviour NPC { get { return npc; } }

    public NpcPath(MonoBehaviour npc)
    {
        this.npc = npc;
        SetLastDestination(NPC.transform.position);
        SetDestination(NPC.transform.position);
    }

    public void SetLastDestination(Vector3 position)
    {
        lastDestination = position;
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

    public bool PathExistsTo(Vector3 destination)
    {
        var tempPath = new NavMeshPath();
        return NavMesh.CalculatePath(NPC.transform.position, destination, NavMesh.AllAreas, tempPath);
    }

    public void Update(Vector3 destination)
    {
        hasArrived = false;

        var deltaDestination = destination - lastDestination;
        if (deltaDestination.sqrMagnitude > 0.1f)
        {
            SetDestination(destination);
            isFinalPoint = false;
        }

        var toCurPathPos = Utility.AtHeight(path[curPathIndex], 0f) - Utility.AtHeight(NPC.transform.position, 0f);
        if (toCurPathPos.sqrMagnitude < 0.1f)
        {
            curPathIndex++;
            isFinalPoint = curPathIndex >= path.Length - 1;
        }

        if (curPathIndex > path.Length - 1)
        {
            //Debug.Log("DESTINATION REACHED!");
            hasArrived = true;
            curPathIndex = path.Length - 1;
        }

        // Debug draw path
        for (var i = 0; i < path.Length - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.magenta);
        }
    }

    public bool IsFinalPathPoint()
    {
        return isFinalPoint;
    }

    public bool HasArrived()
    {
        return hasArrived;
    }

    public Vector3 GetCurrentPathTargetPosition()
    {
        return path[curPathIndex];
    }
}
