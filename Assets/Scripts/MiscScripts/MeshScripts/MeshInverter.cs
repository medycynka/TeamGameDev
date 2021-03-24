using UnityEngine;


namespace SzymonPeszek.Misc
{
    public class MeshInverter : MonoBehaviour
    {
        public bool invertFaces = true;
        public bool invertNormals = true;
        public GameObject toInvert;
        public bool invertMeshCollider = true;

        void Start()
        {
            MeshFilter mf = toInvert.GetComponent<MeshFilter>();

            if (mf != null)
            {
                Mesh m = Instantiate(mf.mesh);
                Process(m);
                mf.mesh = m;
                
                if (invertMeshCollider)
                {
                    toInvert.GetComponent<MeshCollider>().sharedMesh = m;
                }
            }

            SkinnedMeshRenderer smr = toInvert.GetComponent<SkinnedMeshRenderer>();

            if (smr != null)
            {
                Mesh m = Instantiate(smr.sharedMesh);
                Process(m);
                smr.sharedMesh = m;

                if (invertMeshCollider)
                {
                    toInvert.GetComponent<MeshCollider>().sharedMesh = m;
                }
            }
        }

        private void Process(Mesh m)
        {
            int subMeshes = m.subMeshCount;

            for (int i = 0; i < subMeshes; i++)
            {
                if (invertFaces)
                {
                    MeshTopology type = m.GetTopology(i);
                    int[] indices = m.GetIndices(i);

                    if (type == MeshTopology.Quads)
                    {
                        for (int n = 0; n < indices.Length; n += 4)
                        {
                            int tmp = indices[n];
                            indices[n] = indices[n + 3];
                            indices[n + 3] = tmp;
                            tmp = indices[n + 1];
                            indices[n + 1] = indices[n + 2];
                            indices[n + 2] = tmp;
                        }
                    }
                    else if (type == MeshTopology.Triangles)
                    {
                        for (int n = 0; n < indices.Length; n += 3)
                        {
                            int tmp = indices[n];
                            indices[n] = indices[n + 1];
                            indices[n + 1] = tmp;
                        }
                    }

                    m.SetIndices(indices, type, i);
                }
            }

            if (invertNormals)
            {
                Vector3[] normals = m.normals;

                for (int n = 0; n < normals.Length; n++)
                {
                    normals[n] = -normals[n];
                }

                m.normals = normals;
            }

            toInvert.GetComponent<MeshFilter>().mesh = m;
        }
    }
}
