using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration
{
    public class WorldGeneration : MonoBehaviour
    {
        #region Gameplay
        private float _chunkSpawnZ;
        private Queue<Chunk> _activeChunks = new Queue<Chunk>();
        //TODO object pool
        private List<Chunk> _chunkPool = new List<Chunk>();
        #endregion

        #region Configurable fields
        [SerializeField] private int firstChunkSpawnPosition = -10;
        [SerializeField] private int chunkOnScreen = 5;
        [SerializeField] private float despawnDistance = 5.0f;
        #endregion 

        [SerializeField] private List<GameObject> chunkPrefabs;
        [SerializeField] private Transform cameraTransform;

        private void Awake()
        {
            ResetWorld();
        }

        private void Start()
        {
            // Check if we have an empty chunkPrefab list
            if (chunkPrefabs.Count == 0)
            {
                Debug.LogError("No chunk prefab found on the world generator, please assign some chunks!");
                return;
            }

            // Try to assign the cameraTransform
            if (!cameraTransform) 
            {
                cameraTransform = Camera.main.transform;
                Debug.Log("We've assigned cameraTransform automaticly to the Camera.main");
            }
        }

        public void ScanPosition()
        {
            float cameraZ = cameraTransform.position.z;
            Chunk lastChunk = _activeChunks.Peek();

            if (cameraZ >= lastChunk.transform.position.z + lastChunk.ChunkLength + despawnDistance)
            {
                SpawnNewChunk();
                DeleteLastChunk();
            }
        }

        private void SpawnNewChunk()
        {
            // Get a random index for which prefab to spawn
            int randomIndex = Random.Range(0, chunkPrefabs.Count);

            // Does it already exist within our pool
            Chunk chunk = _chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefabs[randomIndex].name + "(Clone)"));

            // Create a chunk, if were not able to find one to reuse
            if (!chunk)
            {
                GameObject go = Instantiate(chunkPrefabs[randomIndex], transform);
                chunk = go.GetComponent<Chunk>();
            }

            // Place the object, and show it
            chunk.transform.position = new Vector3(0, 0, _chunkSpawnZ);
            _chunkSpawnZ += chunk.ChunkLength;

            // Store the value, to reuse in our pool
            _activeChunks.Enqueue(chunk);
            chunk.ShowChunk();
        }

        private void DeleteLastChunk()
        {
            Chunk chunk = _activeChunks.Dequeue();
            chunk.HideChunk();
            _chunkPool.Add(chunk);
        }

        public void ResetWorld()
        {
            // Reset the ChunkSpawn Z
            _chunkSpawnZ = firstChunkSpawnPosition;

            for (int i = _activeChunks.Count; i != 0; i--)
                DeleteLastChunk();

            for (int i = 0; i < chunkOnScreen; i++)
                SpawnNewChunk();
        }
    }
}
