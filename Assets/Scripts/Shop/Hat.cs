using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "Hat")]
    public class Hat : ScriptableObject
    {
        public string ItemName;
        public int ItemPrice;
        public Sprite Thumbnail;
        public GameObject Model;
    }
}