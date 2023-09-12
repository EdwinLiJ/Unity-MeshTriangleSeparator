using System;
using UnityEngine;

namespace Codes {
    public class BreakingEffect : MonoBehaviour {
        private static readonly int LIFE_TIME = Shader.PropertyToID("_LifeTime");

        [SerializeField]
        private Material m_material;

        [SerializeField]
        private Mesh m_mesh;

        [SerializeField]
        [Range(0.01f, 10f)]
        private float m_playingSpeed = 1f;

        private MeshFilter m_meshFilter;
        private MeshRenderer m_meshRenderer;

        private float m_playingTime;

        private void Update() {
            if (m_material && m_mesh) {
                if (!m_meshFilter) {
                    var child = new GameObject("Mesh");
                    child.transform.SetParent(transform);
                    m_meshRenderer = child.AddComponent<MeshRenderer>();
                    m_meshRenderer.sharedMaterial = m_material;

                    m_meshFilter = child.AddComponent<MeshFilter>();
                    m_meshFilter.sharedMesh = m_mesh;
                }


                m_playingTime = Mathf.PingPong(Time.time * m_playingSpeed, 1f);
                m_material.SetFloat(LIFE_TIME, m_playingTime);
            }
        }
    }
}