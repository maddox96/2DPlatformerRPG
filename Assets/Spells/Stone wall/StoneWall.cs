using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimatedPixelPack
{


    public class StoneWall : Spell
    {
        Rigidbody2D rb;
        float direction;
        protected override void Start()
        {
            direction = caster.CurrentDirection == Character.Direction.Left ? -1 : 1;
            rb = GetComponent<Rigidbody2D>();
            base.Start();
            if(caster != null) transform.position = new Vector3(caster.transform.position.x + direction, 0.0f, 0.0f);
            rb.AddForce(new Vector2(direction * 10000000, 0.0f));
            Invoke("BlockMovement", 1.0f);
        }

        void BlockMovement()
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}