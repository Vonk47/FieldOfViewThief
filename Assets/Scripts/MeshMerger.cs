using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshMerger : MonoBehaviour
    {
        public void MergeMeshes()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            MeshFilter currentMeshFilter = transform.GetComponent<MeshFilter>();
            Material targetMaterial = meshFilters[1].gameObject.GetComponent<MeshRenderer>().material;

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }
            currentMeshFilter.mesh = new Mesh();
            currentMeshFilter.mesh.CombineMeshes(combine);
            GetComponent<MeshRenderer>().material = targetMaterial;
            gameObject.SetActive(true);
        }

        public void DeleteChilden()
        {
           foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}


