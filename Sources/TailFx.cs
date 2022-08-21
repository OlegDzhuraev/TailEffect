using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.TailEffect
{
    public class TailFx : MonoBehaviour
    {
        public enum TailAxis { TwoD, ThreeD }
        public enum TailStyle { Hard, Stretchy, Combined }

        [SerializeField] GameObject headPrefab;
        [SerializeField] GameObject segmentPrefab;
        [SerializeField] GameObject tailPrefab;
        [SerializeField] int segments = 3;

        [SerializeField] TailStyle style = TailStyle.Stretchy;
        [SerializeField] TailAxis followAxis = TailAxis.ThreeD;
        [SerializeField, Range(0.1f, 250f)] float stretchySpeed = 10f;
        [SerializeField, Range(0f, 100f)] float spaceBetween = 1f;
        [SerializeField] bool rotateToPrevious = true;

        readonly List<Transform> parts = new List<Transform>();

        void Start()
        {
            parts.Add(MakePart(headPrefab).transform);

            for (int i = 0; i < segments; i++)
                parts.Add(MakePart(segmentPrefab).transform);

            parts.Add(MakePart(tailPrefab).transform);
        }

        GameObject MakePart(GameObject prefab) => Instantiate(prefab, transform.position, Quaternion.identity);

        void Update()
        {
            parts[0].transform.position = transform.position;
            parts[0].transform.rotation = transform.rotation;
            
            for (var i = 1; i < parts.Count; i++)
            {
                var previous = parts[i - 1];
                var self = parts[i];

                if (style == TailStyle.Hard || style == TailStyle.Combined)
                {
                    var direction = (previous.position - self.position).normalized;
                    self.position = previous.position - direction * spaceBetween;
                }
                
                if (style == TailStyle.Stretchy || style == TailStyle.Combined)
                { 
                    var offsetDirection = GetDirection(rotateToPrevious ? previous : parts[0]);
                    var targetPos = previous.position - offsetDirection * spaceBetween;
                    self.position = Vector3.Lerp(self.position, targetPos, Time.smoothDeltaTime * stretchySpeed);
                }

                if (!rotateToPrevious)
                    continue;
                
                if (followAxis == TailAxis.ThreeD)
                    self.LookAt(previous.position);
                else
                    LookAt2D(self, previous.position);
            }
        }
        
        Vector3 GetDirection(Transform tr)
        {
            return followAxis switch
            {
                TailAxis.ThreeD => tr.forward,
                TailAxis.TwoD => tr.up,
                _ => default
            };
        }
        
        void LookAt2D(Transform tr, Vector3 position)
        {
            var direction = (position - tr.position).normalized;
            tr.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }
}
