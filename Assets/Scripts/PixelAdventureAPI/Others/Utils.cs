using UnityEngine;

namespace PixelAdventureAPI.Others
{
    public class Utils
    {
        /// <summary>
        /// Also draws the Physics2D.BoxCast(...);
        /// </summary>
        static public RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
        {
            RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask);

            //Setting up the points to draw the cast
            Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
            float w = size.x * 0.5f;
            float h = size.y * 0.5f;
            p1 = new Vector2(-w, h);
            p2 = new Vector2(w, h);
            p3 = new Vector2(w, -h);
            p4 = new Vector2(-w, -h);

            Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
            p1 = q * p1;
            p2 = q * p2;
            p3 = q * p3;
            p4 = q * p4;

            p1 += origin;
            p2 += origin;
            p3 += origin;
            p4 += origin;

            Vector2 realDistance = direction.normalized * distance;
            p5 = p1 + realDistance;
            p6 = p2 + realDistance;
            p7 = p3 + realDistance;
            p8 = p4 + realDistance;


            //Drawing the cast
            Color castColor = hit ? Color.green : Color.red;
            Debug.DrawLine(p1, p2, castColor);
            Debug.DrawLine(p2, p3, castColor);
            Debug.DrawLine(p3, p4, castColor);
            Debug.DrawLine(p4, p1, castColor);

            Debug.DrawLine(p5, p6, castColor);
            Debug.DrawLine(p6, p7, castColor);
            Debug.DrawLine(p7, p8, castColor);
            Debug.DrawLine(p8, p5, castColor);

            Debug.DrawLine(p1, p5, Color.grey);
            Debug.DrawLine(p2, p6, Color.grey);
            Debug.DrawLine(p3, p7, Color.grey);
            Debug.DrawLine(p4, p8, Color.grey);
            if (hit)
            {
                Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
            }

            return hit;
        }

        /// Also draws the Phyisics2D.RayCast(...);
        static public RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

            Vector3 end = origin;
            if (direction == Vector2.left)
                end.x -= distance;
            else if (direction == Vector2.right)
                end.x += distance;
            else if (direction == Vector2.up)
                end.y += distance;
            else if (direction == Vector2.down)
                end.y -= distance;

            Debug.DrawLine
            (
                start : origin,
                end : end,
                color : hit.collider != null ? Color.red : Color.green
            );

            return hit;
        }
    }

    
}


