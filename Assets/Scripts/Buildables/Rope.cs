using UnityEngine;

public class Rope : Buildable
{
    [SerializeField] private GameObject _segmentPrefab;

    void Awake() {
        if (!Physics.autoSimulation) {
            Physics.autoSimulation = true;
        }
    }

    void Start() {
        transform.position = transform.position - new Vector3(0, 3f, 0);
        Soor.RopeGenerator.RopeData ropeData = new Soor.RopeGenerator.RopeData()
        {
            ropeLength = 3f,
            ropeThickness = 0.08f,
            segmentsDistance = 0.08f,
            segmentPrefab = _segmentPrefab,
            freezeLastRopeSegment = true,
        };

        Soor.RopeGenerator.Rope rope = new Soor.RopeGenerator.Rope(ropeData, this.gameObject);
        rope.SpawnRope();
    }
}
