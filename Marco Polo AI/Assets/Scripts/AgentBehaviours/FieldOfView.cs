using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Perception))]
public class FieldOfView : MonoBehaviour
{
    // The radius/distance the agent can see
    public float ViewRadius;

    // The angle the agent can see in degrees. 0 to 360
    [Range(0, 360)]
    public float ViewAngle;

    // The layer of target objects the agent is looking for
    public LayerMask TargetLayer;

    // The layer of objects that wil block LoS
    public LayerMask ObstacleLayer;

    // List of visible targets updated at a set interval
    public List<Transform> VisibleTargets = new List<Transform>();

    //Variables for drawing a mesh
    public float MeshResolution;
    public int EdgeResolveIterations;
    public float EdgeDistanceThreshold;
    public MeshFilter ViewMF;
    Mesh ViewMesh;
    public bool DrawFOV = true;


    void Start()
    {
        // Initialise drawing of mesh
        ViewMesh = new Mesh();
        ViewMesh.name = "View Mesh";
        ViewMF.mesh = ViewMesh;

        // Checking for targets every 0.2 seconds. Approximately the average human response time to stimulus
        InvokeRepeating("FindVisibleTargets", 0.2f, 0.2f);
    }

    // Update is called once per frame
    void FindVisibleTargets()
    {
        // Clear current visible targets
        VisibleTargets.Clear();

        // Sphere collision check for nearby targets
        Collider[] targets = Physics.OverlapSphere(transform.position, ViewRadius, TargetLayer);

        //Iterate through each target
        foreach (Collider target in targets)
        {
            // Get direction and magnitude to a target
            Vector3 DirToTarget = (target.transform.position - transform.position);

            // Normalise to get direction without magnitude
            Vector3 DirToTargetNormalized = DirToTarget.normalized;

            if (Vector3.Angle(transform.forward, DirToTargetNormalized) < ViewAngle / 2 // Check if target is within FoV
                && !Physics.Raycast(transform.position, DirToTargetNormalized, DirToTarget.magnitude, ObstacleLayer)) // Raycast to determine LoS
            {
                // Target found
                VisibleTargets.Add(target.transform);
            }
        }

        // Add the memory record to our perception system
        Perception percept = GetComponent<Perception>();

        percept.ClearFoV();
        foreach(Transform target in VisibleTargets)
        {
            percept.AddMemory(target.gameObject);
        }
    }

    private void LateUpdate()
    {
        // Draw/Hide the FoV
        if (DrawFOV)
        {
            ViewMF.gameObject.SetActive(true);
            DrawFieldOfView();

            foreach(Transform target in VisibleTargets)
            {
                Debug.DrawLine(transform.position, target.position, Color.magenta);
            }
        }

        else
        {
            ViewMF.gameObject.SetActive(false);
        }
    }

    // Drawing the FOV for debug purposes
    void DrawFieldOfView()
    {
        int StepCount = Mathf.RoundToInt(ViewAngle * MeshResolution);
        float StepAngleSize = ViewAngle / StepCount;
        List<Vector3> ViewPoints = new List<Vector3>();
        ViewCastInfo OldViewCast = new ViewCastInfo();

        for (int i =0; i <= StepCount; i++)
        {
            float angle = transform.eulerAngles.y - ViewAngle / 2 + StepAngleSize * i;
            ViewCastInfo NewViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool EdgeDistThresholdExceeded = Mathf.Abs(OldViewCast.dist - NewViewCast.dist) > EdgeDistanceThreshold;

                if (OldViewCast.hit != NewViewCast.hit || (OldViewCast.hit && NewViewCast.hit && EdgeDistThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(OldViewCast, NewViewCast);

                    if (edge.pointA != Vector3.zero)
                        ViewPoints.Add(edge.pointA);

                    if (edge.pointB != Vector3.zero)
                        ViewPoints.Add(edge.pointB);
                }
            }
            
            ViewPoints.Add(NewViewCast.point);
            OldViewCast = NewViewCast;
        }

        int VertexCount = ViewPoints.Count + 1;
        Vector3[] vertices = new Vector3[VertexCount];
        int[] triangles = new int[(VertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < VertexCount - 1; i++)
        {
            {
                vertices[i + 1] = transform.InverseTransformPoint(ViewPoints[i]);

                if (i < VertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            ViewMesh.Clear();

            ViewMesh.vertices = vertices;
            ViewMesh.triangles = triangles;
            ViewMesh.RecalculateNormals();
        }
    }

    ViewCastInfo ViewCast(float GlobalAngle)
    {
        Vector3 dir = DirFromAngle(GlobalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, ViewRadius, ObstacleLayer))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, GlobalAngle);
        }

        else
        {
            return new ViewCastInfo(false, transform.position + dir * ViewRadius, ViewRadius, GlobalAngle);
        }
    }

    EdgeInfo FindEdge(ViewCastInfo MinViewCast, ViewCastInfo MaxViewCast)
    {
        float MinAngle = MinViewCast.angle;
        float MaxAngle = MaxViewCast.angle;
        Vector3 MinPoint = Vector3.zero;
        Vector3 MaxPoint = Vector3.zero;

        for (int i = 0; i < EdgeResolveIterations; i++)
        {
            float angle = (MinAngle + MaxAngle) / 2;
            ViewCastInfo NewViewCast = ViewCast(angle);

            bool EdgeDistThresholdExceeded = Mathf.Abs(MinViewCast.dist - NewViewCast.dist) > EdgeDistanceThreshold;

            if (NewViewCast.hit == MinViewCast.hit && !EdgeDistThresholdExceeded)
            {
                MinAngle = angle;
                MinPoint = NewViewCast.point;
            }

            else
            {
                MaxAngle = angle;
                MaxPoint = NewViewCast.point;
            }
        }

        return new EdgeInfo(MinPoint, MaxPoint);
    }

    public Vector3 DirFromAngle(float AngleInDegrees, bool AngleIsGlobal)
    {
        if (!AngleIsGlobal)
        {
            AngleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(AngleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(AngleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
