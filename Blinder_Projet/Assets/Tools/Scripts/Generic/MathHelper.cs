using UnityEngine;

namespace Tools
{
    /// <summary>
    /// Gather all the usefull methods around globalbounds
    /// </summary>
    public static class MathHelper
    {
        public static readonly float TwoSqrt = Mathf.Sqrt(2);

        /// <summary>
        /// Construct the movement from the direction and speed.
        /// We also check if the offset is long enough (greater than min)
        ///  to be relevant
        /// </summary>
        /// <param name="offsetWithTarget">The difference between target and source</param>
        /// <param name="speed">The final speed of the movement</param>
        /// <param name="min">the mininum distance to make relevant the offset</param>
        /// <returns>The movement</returns>
        public static Vector2 ConstructMovement(Vector2 offsetWithTarget, float speed, float min)
        {
            if (offsetWithTarget.sqrMagnitude > min)
            {
                offsetWithTarget.Normalize();
                offsetWithTarget *= speed;
            }
            else
            {
                offsetWithTarget = Vector2.zero;
            }

            return offsetWithTarget;
        }


        /// <summary>
        /// Rotate a vector of x radian.
        /// </summary>
        /// <param name="vector2">The vector to rotate</param>
        /// <param name="angleInRadian">The angle in radiant</param>
        /// <returns>The rotated vector</returns>
        /// <source>http://stackoverflow.com/questions/4780119/2d-euclidean-vector-rotations</source>
        public static Vector2 RotateVector(Vector2 vector2, float angleInRadian)
        {
            float cos = Mathf.Cos(angleInRadian);
            float sin = Mathf.Sin(angleInRadian);

            Vector2 result = new Vector2();

            result.x = vector2.x * cos - vector2.y * sin;
            result.y = vector2.x * sin + vector2.y * cos;

            return result;
        }

        /// <summary>
        /// Return a normalized direction vector from the given angle in radian
        /// </summary>
        /// <param name="angleInRadian">The given angle</param>
        /// <returns>The normalized vector</returns>
        /// <source>http://math.stackexchange.com/questions/180874/convert-angle-radians-to-a-heading-vector</source>
        public static Vector2 GetDirectionFromAngle(float angleInRadian)
        {
            return new Vector2(Mathf.Cos(angleInRadian),
                Mathf.Sin(angleInRadian));
        }

        /// <summary>
        /// Return an angle between -90 and 90 degrees representing the smallest difference between the two vector
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <source>http://answers.unity3d.com/questions/24983/how-to-calculate-the-angle-between-two-vectors.html</source>
        public static float Angle(Vector2 from, Vector2 to)
        {
            return Mathf.DeltaAngle(Mathf.Atan2(from.y, from.x) * Mathf.Rad2Deg,
                                    Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg);
        }

        /// <summary>
        /// Return an angle between -90 and 90 degrees representing the smallest difference between the two vector
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <source>http://answers.unity3d.com/questions/24983/how-to-calculate-the-angle-between-two-vectors.html</source>
        public static float Angle(Vector2 vector2)
        {
            return Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Wraps a value around some significant range.
        ///
        /// Similar to modulo, but works in a unary direction over any range (including negative values).
        ///
        /// ex:
        /// Wrap(8,6,2) == 4
        /// Wrap(4,2,0) == 0
        /// Wrap(4,2,-2) == -2
        /// </summary>
        /// <param name="value">value to wrap</param>
        /// <param name="max">max in range</param>
        /// <param name="min">min in range</param>
        /// <returns>A value wrapped around min to max</returns>
        /// <remarks></remarks>
        public static float Wrap(float value, float max, float min)
        {
            value -= min;
            max -= min;
            if (max == 0)
                return min;

            value = value % max;
            value += min;
            while (value < min)
            {
                value += max;
            }

            return value;

        }

        /// <summary>
        /// set an angle with in the bounds of -PI to PI
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float NormalizeAngle(float angle, bool useRadians)
        {
            float rd = (useRadians ? Mathf.PI : 180);
            return Wrap(angle, rd, -rd);
        }

        /// <summary>
        /// Shoot a point on a segment (same trajectory)
        /// </summary>
        /// <param name="origin">origin this point will determine the sens of the projection it will be the closer point of the new shooted point)</param>
        /// <param name="direction">direction this point determine the direction (it allowed to get a segment)</param>
        /// <param name="distance">distance the distance between the origin and the new shooted point</param>
        /// <returns>the position of the new shooted point</returns>
        public static Vector2 ShootPoint(Vector2 origin, Vector2 direction, float distance)
        {
            float segmentLength = Vector2.Distance(origin, direction);

            // place the point at the width distance of the end of the edge
            float deltaX = distance * (direction.x - origin.x) / segmentLength;
            float deltaY = distance * (direction.y - origin.y) / segmentLength;

            float newX = origin.x + deltaX;
            float newY = origin.y + deltaY;

            return new Vector2(newX, newY);
        }

        /// <summary>
        /// Return a specific point on the circumference of a circle, from a given angle
        /// </summary>
        /// <param name="circleCenter">The center position of the circle</param>
        /// <param name="radius">The circle radius</param>
        /// <param name="angleInRadian">The angle in radian</param>
        /// <returns>The point on the circumference of the circle</returns>
        /// <source>http://gamedev.stackexchange.com/questions/18340/get-position-of-point-on-circumference-of-circle-given-an-angle</source>
        public static Vector2 GetPointOnCircle(Vector2 circleCenter, float radius, float angleInRadian)
        {
            Vector2 newPoint = Vector2.zero;

            newPoint.x = Mathf.Cos(angleInRadian) * radius + circleCenter.x;
            newPoint.y = Mathf.Sin(angleInRadian) * radius + circleCenter.y;

            return newPoint;
        }

        /// <summary>
        /// Compute int pow by a optimized way
        /// </summary>
        /// <param name="baseValue">The value which we want to compute</param>
        /// <param name="exp">The exponential value</param>
        /// <returns>The int result</returns>
        /// <source>http://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int</source>
        public static int Pow(int baseValue, int exp)
        {
            int result = 1;

            while (exp != 0)
            {
                if ((exp & 1) != 0)
                    result *= baseValue;
                exp >>= 1;
                baseValue *= baseValue;
            }

            return result;
        }

        /// <summary>
        /// Compute the power of two from a given expo
        /// </summary>
        /// <param name="power">The expo</param>
        /// <returns>The result</returns>
        public static int PowerOfTwo(int power)
        {
            return Pow(2, power);
        }

        /// <summary>
        /// find the nearest point on the perimeter of the rectangle to the outside point
        /// </summary>
        /// <param name="rect">The rect</param>
        /// <param name="outsidePoint"></param>
        /// <returns></returns>
        /// <source>http://stackoverflow.com/questions/20453545/how-to-find-the-nearest-point-in-the-perimeter-of-a-rectangle-to-a-given-point</source>
        public static Vector2 ClosestPointToRect(Rect rect, Vector2 outsidePoint)
        {
            return ClosestPointToRect(rect.xMin, rect.xMax, rect.yMax, rect.yMin, outsidePoint);
        }

        public static Vector2 ClosestPointToRect(float left, float right, float top, float bottom, Vector2 outsidePoint)
        {
            outsidePoint.x = Mathf.Clamp(outsidePoint.x, left, right);
            outsidePoint.y = Mathf.Clamp(outsidePoint.y, bottom, top);

            float deltaLeft = Mathf.Abs(outsidePoint.x - left);
            float deltaRight = Mathf.Abs(outsidePoint.x - right);
            float deltaTop = Mathf.Abs(outsidePoint.y - top);
            float deltaBottom = Mathf.Abs(outsidePoint.y - bottom);

            float min = Mathf.Min(deltaLeft, deltaRight, deltaTop, deltaBottom);

            if (min == deltaTop)
                return new Vector2(outsidePoint.x, top);

            if (min == deltaBottom)
                return new Vector2(outsidePoint.x, bottom);

            if (min == deltaLeft)
                return new Vector2(left, outsidePoint.y);

            return new Vector2(right, outsidePoint.y);
        }

        /// <summary>
        /// find the nearest point on the perimeter of the rectangle to the outside point
        /// </summary>
        /// <param name="rect">The rect</param>
        /// <param name="outsidePoint"></param>
        /// <returns></returns>
        /// <source>http://stackoverflow.com/questions/20453545/how-to-find-the-nearest-point-in-the-perimeter-of-a-rectangle-to-a-given-point</source>
        public static Vector2 ClosestPointToBounds(Bounds bounds, Vector2 outsidePoint)
        {
            return ClosestPointToRect(bounds.min.x, bounds.max.x, bounds.max.y, bounds.min.y, outsidePoint);
        }
    }
}
