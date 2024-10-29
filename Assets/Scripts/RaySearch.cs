using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Code collaboration with Freya Holmér
//https://twitter.com/FreyaHolmer

public class RaySearch : MonoBehaviour
{
   // public string[] obstaclesTags;
    public float stepSize = 0.1f;
    public float offsetMargin = 0.01f;
    //public int checkCountMax = 100;
    private bool cornerCheck = false;
    public List<MeshPoint> meshPoints = new List<MeshPoint>();
    public List<MeshPoint> cornerPoints = new List<MeshPoint>();// 这没初始化导致AddComponent出来的该类此数组报空
   // public List<Vector3> pathPoints = new List<Vector3>();

    List<Vector3[]> debugTangentCheck = new List<Vector3[]>();
    List<Vector3[]> debugNegativeCheck = new List<Vector3[]>();
    List<Vector3[]> debugBehindCheck = new List<Vector3[]>();
    private void Start()
    {
       // DoPoints();
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.white;
#endif
        //Handles.DrawAAPolyLine(cornerPoints);
        DrawLinePairs(debugTangentCheck, Color.red);
        DrawLinePairs(debugNegativeCheck, Color.blue);
        DrawLinePairs(debugBehindCheck, Color.cyan);

        if (cornerPoints == null)
            return;

        foreach(MeshPoint p in cornerPoints)
        {
            Gizmos.DrawWireSphere(p.position, .15f);
        }
    }

    void DrawLinePairs(List<Vector3[]> list, Color color)
    {
#if UNITY_EDITOR
        Handles.color = color;
        foreach (Vector3[] pair in list)
        {
            Handles.DrawAAPolyLine(pair[0], pair[1]);
        }
#endif
    }

    bool IsObstacle(GameObject gameObject)
    {
        foreach(string tag in BaseSetting.GetInstance.ObstaclesTags)
        {
            if (gameObject.tag == tag)
                return true;
        }
        return false;
    }


    void FindNext(Vector3 pt, Vector3 normal)
    {
        MeshPoint mp = new MeshPoint(pt,normal); //mp.position = pt; mp.normal = normal;

        if (meshPoints.Count > 1)
        {

            //MeshPoint mpnew = new MeshPoint(pt,normal); //mpnew.position = pt; mpnew.normal = normal;
            if (Vector3.Distance(meshPoints[0].position, mp.position) <= stepSize && meshPoints[0].normal == normal)//LZX 2023/11/21
            {
                return;
            }
            if (cornerPoints.Count > 0)
            {
                if (Vector3.Distance(cornerPoints[0].position, mp.position) < .3f && cornerPoints[0].normal == normal)
                    cornerCheck = true;
            }

            print(Vector3.Dot(meshPoints[meshPoints.Count - 1].normal, normal));

            if (Vector3.Dot(meshPoints[meshPoints.Count - 1].normal, normal) < .98f && !cornerCheck)
                cornerPoints.Add(mp);
        }

        meshPoints.Add(mp);

        Vector3 tangent = Vector3.Cross(normal, Vector3.up);
        Vector3 offsetPt = pt + normal * offsetMargin;
        Vector3 tangentCheckPoint = offsetPt + tangent * stepSize;
        Vector3 negativeCheckPoint = tangentCheckPoint - normal * (offsetMargin * 2);
        Vector3 behindCheckPoint = negativeCheckPoint - tangent * (stepSize * 0.75f);

        // find positive turn or flat surface
        bool foundThing = false;
        RaycastHit hit;

        // Check positive turn
        if (Physics.Raycast(offsetPt, tangent, out hit, stepSize) && !IsObstacle(hit.collider.gameObject))
        {
            debugTangentCheck.Add(new[] { offsetPt, hit.point });
            foundThing = true;
        }
        else
        {
            debugTangentCheck.Add(new[] { offsetPt, tangentCheckPoint });
            // check flat or slight negative turn
            if (Physics.Raycast(tangentCheckPoint, -normal, out hit, offsetMargin * 2) && !IsObstacle(hit.collider.gameObject))
            {
                debugNegativeCheck.Add(new[] { tangentCheckPoint, hit.point });
                foundThing = true;
            }
            else
            {
                debugNegativeCheck.Add(new[] { tangentCheckPoint, negativeCheckPoint });
                // check negative turn
                if (Physics.Raycast(negativeCheckPoint, -tangent, out hit, stepSize * 2) && !IsObstacle(hit.collider.gameObject))
                {
                    foundThing = true;
                    debugBehindCheck.Add(new[] { negativeCheckPoint, hit.point });
                }
                else
                {
                    debugBehindCheck.Add(new[] { negativeCheckPoint, behindCheckPoint });
                }
            }
        }

        if (foundThing)//LZX 2023/11/21 //&& meshPoints.Count < checkCountMax)
        {
            FindNext(hit.point, hit.normal);
        }

    }

    [ContextMenu("Find Points")]
    public void DoPoints()
    {
        cornerCheck = false;

        if (meshPoints.Count > 0)
        {
            meshPoints.Clear();
            cornerPoints.Clear();
            debugTangentCheck.Clear();
            debugNegativeCheck.Clear();
            debugBehindCheck.Clear();
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            FindNext(hit.point, hit.normal);

        //create array for white line
        //cornerPoints = new Vector3[meshPoints.Count];
        //for (int i = 0; i < meshPoints.Count; i++)
        //    cornerPoints[i] = meshPoints[i].position;
    }

}

