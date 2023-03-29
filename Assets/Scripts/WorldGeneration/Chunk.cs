﻿using UnityEngine;

namespace WorldGeneration
{
    public class Chunk : MonoBehaviour
    {
        public float ChunkLength;

        public Chunk ShowChunk()
        {
            transform.gameObject.BroadcastMessage("OnShowChunk", SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(true);
            return this;
        }

        public Chunk HideChunk()
        {
            gameObject.SetActive(false);
            return this;
        }
    }
}
