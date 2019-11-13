using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public ParticleSystem part;
    public float xTarget;
    Vector2 targetPosition;
    public SpriteRenderer _sprite;
    float speed;

    void Start()
    {
       
        float size = Random.Range(transform.localScale.x, transform.localScale.x * 2);

       
        speed = Random.Range(3f, 4f);
        if (_sprite.sortingLayerName == "Above")
            speed = 12f;


        transform.localScale = new Vector3(size, size, size);
        targetPosition = new Vector2(xTarget, transform.position.y);

        var shape = part.shape;
        shape.radius = size * 2f;
        Destroy(gameObject, 80f);


    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
