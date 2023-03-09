using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed = 5;
    private void Update() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(input != Vector2.zero) {
            transform.Translate(new Vector3(input.x * speed * Time.deltaTime, 0, input.y * speed * Time.deltaTime));
        }
    }
}
