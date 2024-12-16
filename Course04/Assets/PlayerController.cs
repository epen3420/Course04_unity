using UnityEngine;

[RequireComponent(typeof(CharaController))]
public class PlayerController : MonoBehaviour
{
    CharaController charaCon;


    private void Start()
    {
        charaCon = GetComponent<CharaController>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, z).normalized;

        charaCon.Move(direction);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            charaCon.Sprint();
        }
    }
}
