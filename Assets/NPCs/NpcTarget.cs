using UnityEngine;

public class NpcTarget : MonoBehaviour
{
    public Team Team;

    private void Awake()
    {
        TargetingUtility.AddTargetable(Team, transform);
    }

    private void OnDestroy()
    {
        TargetingUtility.RemoveTargetable(Team, transform);
    }

    public Team OpposingTeam
    {
        get
        {
            if (Team == Team.Good)
                return Team.Bad;
            return Team.Good;
        }
    }
}
