using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshCollider))]
[RequireComponent (typeof (MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] int xSize = 20;
    [SerializeField] int zSize = 20;
    [Range (0, 8)]
    [SerializeField] int vertFactor = 2;
    [SerializeField] Gradient gradient;
    [SerializeField] float minTerrainHeight = 0f;
    [SerializeField] float maxTerrainHeight = 3;

    [Range (0, 0.5f)]
    [SerializeField] float factor = 0.1f;

    [Range (0, 5f)]
    [SerializeField] float heightFactor = 2f;

    Mesh mesh;

    Vector3 [] verticies;
    int [] triangles;
    Color [] colors;


    // Start is called before the first frame update
    void OnEnable ()
    {
        refresh ();
    }

    void refresh ()
    {
        mesh = new Mesh ();
        GetComponent<MeshFilter> ().mesh = mesh;
        createShape ();
        updateMesh ();
    }

    private void OnValidate ()
    {
        refresh ();
    }

    void updateMesh ()
    {
        mesh.Clear ();

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals ();

        GetComponent<MeshCollider> ().sharedMesh = GetComponent<MeshFilter> ().sharedMesh;
    }

    void createShape ()
    {
        int xSizeFactored = xSize * vertFactor;
        int zSizeFactored = zSize * vertFactor;

        verticies = new Vector3 [(xSizeFactored + 1) * (zSizeFactored + 1)];

        for (int i = 0, z = 0; z <= zSizeFactored; z++)
        {
            for (int x = 0; x <= xSizeFactored; x++)
            {
                float y = Mathf.PerlinNoise (x * factor, z * factor) * heightFactor;

                if (y > maxTerrainHeight)
                {
                    y = maxTerrainHeight;
                }
                else if (y < minTerrainHeight)
                {
                    y = minTerrainHeight;
                }

                verticies [i] = new Vector3 ((float) x / vertFactor, y, (float) z / vertFactor);
                i++;
            }
        }

        triangles = new int [xSizeFactored * zSizeFactored * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSizeFactored; z++)
        {
            for (int x = 0; x < xSizeFactored; x++)
            {
                triangles [tris + 0] = vert + 0;
                triangles [tris + 1] = vert + xSizeFactored + 1;
                triangles [tris + 2] = vert + 1;
                triangles [tris + 3] = vert + 1;
                triangles [tris + 4] = vert + xSizeFactored + 1;
                triangles [tris + 5] = vert + xSizeFactored + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }

        colors = new Color [verticies.Length];

        if (gradient != null)
        {
            for (int i = 0, z = 0; z <= zSizeFactored; z++)
            {
                for (int x = 0; x <= xSizeFactored; x++)
                {
                    float height = Mathf.InverseLerp (minTerrainHeight, maxTerrainHeight, verticies [i].y);
                    colors [i] = gradient.Evaluate (height);
                    i++;
                }
            }
        }
    }
}
