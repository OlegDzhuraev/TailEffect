using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.TailEffect
{
    public enum TailAxis { TwoD, ThreeD }
    public enum TailStyle { Hard, Stretchy, Combined }
    
    public class TailFx : MonoBehaviour
    {
        public event Action Loaded;
        
        [SerializeField] GameObject headPrefab;
        [SerializeField] GameObject segmentPrefab;
        [SerializeField] GameObject tailPrefab;
        [SerializeField, Range(1, 100)] int segments = 3;

        [SerializeField] TailStyle style = TailStyle.Stretchy;
        [SerializeField] TailAxis followAxis = TailAxis.ThreeD;
        [SerializeField, Range(0.1f, 250f)] float stretchySpeed = 10f;
        [SerializeField, Range(0f, 100f)] float spaceBetween = 1f;
        [SerializeField] bool rotateToPrevious = true;

        public List<Transform> Parts => parts;
        
        readonly List<Transform> parts = new List<Transform>();
        Transform head;

        void Start()
        {
            head = MakePart(headPrefab);

            for (var i = 0; i < segments; i++)
                MakePart(segmentPrefab);

            MakePart(tailPrefab);
            
            Loaded?.Invoke();
        }
        
        void Update()
        {
            head.position = transform.position;
            head.rotation = transform.rotation;
            
            for (var i = 1; i < Parts.Count; i++)
            {
                var previous = Parts[i - 1];
                var self = Parts[i];
                
                var previousPos = previous.position;
                
                if (style == TailStyle.Hard || style == TailStyle.Combined)
                {
                    var direction = (previousPos - self.position).normalized;
                    self.position = previousPos - direction * spaceBetween;
                }
                
                if (style == TailStyle.Stretchy || style == TailStyle.Combined)
                { 
                    var offsetDirection = GetDirection(rotateToPrevious ? previous : Parts[0]);
                    var targetPos = previousPos - offsetDirection * spaceBetween;
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
        
        Transform MakePart(GameObject prefab)
        {
            var part = Instantiate(prefab, transform.position, Quaternion.identity).transform;
            Parts.Add(part);
            
            return part;
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

        static void LookAt2D(Transform tr, Vector3 position)
        {
            var direction = (position - tr.position).normalized;
            tr.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }
}
