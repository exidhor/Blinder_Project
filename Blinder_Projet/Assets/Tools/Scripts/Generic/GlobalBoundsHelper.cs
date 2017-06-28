using UnityEngine;

namespace Tools
{
    /// <summary>
    /// Gather all the usefull methods around globalbounds
    /// </summary>
    public static class GlobalBoundsHelper
    {
        private static bool _globalBoundsIsFilled;
        private static Bounds _globalBounds;

        /// <summary>
        /// Return the smallest rect wich contains this two points.
        /// </summary>
        /// <param name="firstPoint">a point</param>
        /// <param name="secondPoint">another point</param>
        /// <returns></returns>
        public static Rect GetGlobalRect(Vector2 firstPoint, Vector2 secondPoint)
        {
            Vector2 min, max;

            if (firstPoint.x < secondPoint.x)
            {
                min.x = firstPoint.x;
                max.x = secondPoint.x;
            }
            else
            {
                min.x = secondPoint.x;
                max.x = firstPoint.x;
            }

            if (firstPoint.y < secondPoint.y)
            {
                min.y = firstPoint.y;
                max.y = secondPoint.y;
            }
            else
            {
                min.y = secondPoint.y;
                max.y = firstPoint.y;
            }

            Vector2 size = max - min;

            return new Rect(min, size);
        }

        /// <summary>
        /// Search recursivly all the bounds from the given type.
        /// It merge it to get the global bounds of an object and all his descendants together.
        /// </summary>
        /// <param name="gameObject">The GO parent</param>
        /// <param name="type">The type to look for</param>
        /// <returns></returns>
        public static Bounds FindGlobalBounds(GameObject gameObject, EBoundsType type)
        {
            _globalBoundsIsFilled = false; // this flag is here to tell that the globalBounds buffer content is irelevant.

            AddGameObjectToGlobalBounds(gameObject, type);

            return _globalBounds; // this buffer is filled in "AddGameObjectToGlobalBounds
        }

        /// <summary>
        /// Add a GameObject bounds to the static global bounds buffer, and then call this method
        /// for his children
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="type"></param>
        private static void AddGameObjectToGlobalBounds(GameObject gameObject, EBoundsType type)
        {
            // we only look for the given type
            switch (type)
            {
                case EBoundsType.Collider:
                    Collider2D collider = gameObject.GetComponent<Collider2D>();

                    if (collider != null)
                    {
                        AddBounds(collider.bounds);
                    }
                    break;

                case EBoundsType.Render:
                    Renderer renderer = gameObject.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        AddBounds(renderer.bounds);
                    }
                    break;
            }

            // we recursivly call this method on every child
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                AddGameObjectToGlobalBounds(gameObject.transform.GetChild(i).gameObject, type);
            }
        }

        /// <summary>
        /// Set the bounds if there is any, Encapsulate the new bounds if not.
        /// </summary>
        /// <param name="bounds">The new bounds to add</param>
        private static void AddBounds(Bounds bounds)
        {
            if (_globalBoundsIsFilled)
            {
                _globalBounds.Encapsulate(bounds);
            }
            else
            {
                // if there is any, we have to set the globalBounds to the first bounds
                // to avoid the encapsulation of the point (0,0) (the default position of a void bounds)
                _globalBounds = bounds;

                _globalBoundsIsFilled = true;
            }
        }
    }
}
