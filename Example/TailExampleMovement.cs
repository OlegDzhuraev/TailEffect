using UnityEngine;

namespace InsaneOne.TailEffect.Example
{
    public class TailExampleMovement : MonoBehaviour
    {
        const float Speed = 5f;
        const float AngularSpeed = 120f;
        const float ResetPos = 20f;

        Vector3 startPos;
        
        void Start() => startPos = transform.position;

        void Update()
        {
            var direction = (transform.forward + transform.up / 2f).normalized;
            
            transform.position += direction * Time.smoothDeltaTime * Speed;
            transform.Rotate(0, AngularSpeed * Time.smoothDeltaTime, 0);

            if (transform.position.y > ResetPos)
                transform.position = startPos;
        }
    }
}