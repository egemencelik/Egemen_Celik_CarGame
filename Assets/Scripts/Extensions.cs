using UnityEngine;

public static class Extensions
{
    public static void SetVelocity(this Rigidbody2D rb, float velX, float velY)
    {
        rb.velocity = new Vector2(velX, velY);
    }
}
