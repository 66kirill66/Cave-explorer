using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    private CharacterController controller;
    public float gravity = 9.81f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!controller.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -2f; // Прижимаем к земле
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
