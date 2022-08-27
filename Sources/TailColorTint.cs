using UnityEngine;

namespace InsaneOne.TailEffect
{
    public class TailColorTint : MonoBehaviour
    {
        [SerializeField] Gradient colorByLength = new Gradient();
        [SerializeField] string materialAttribute = "_Color";
        [SerializeField] bool colorizeOnStart;
        
        TailFx tail;

        void Awake()
        {
            tail = GetComponent<TailFx>();
            
            if (colorizeOnStart)
                tail.Loaded += OnTailLoaded;
        }
        
        void OnDestroy()
        {
            if (colorizeOnStart && tail)
                tail.Loaded -= OnTailLoaded;
        }
        
        void OnTailLoaded() => UpdateColor();
        
        public void UpdateColor()
        {
            for (var i = 0; i < tail.Parts.Count; i++)
            {
                var renderer = tail.Parts[i].GetComponent<Renderer>();

                if (!renderer)
                    renderer = GetComponentInChildren<Renderer>();
                
                if (renderer)
                {
                    var color = colorByLength.Evaluate((float) i / tail.Parts.Count);
                    renderer.material.SetColor(materialAttribute, color);
                }
            }
        }
        
        public void SetCustomColor(Gradient gradient)
        {
            colorByLength = gradient;
            UpdateColor();
        }
    }
}