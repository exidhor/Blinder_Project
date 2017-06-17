using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools
{
    public static class GlobalBoundsHelper
    {
        private static bool _globalBoundsIsFilled;
        private static Bounds _globalBounds;

        public static Bounds FindGlobalBounds(GameObject gameObject, EBoundsType type)
        {
            _globalBoundsIsFilled = false;

            AddGameObjectToGlobalBounds(gameObject, type);

            return _globalBounds;
        }

        private static void AddGameObjectToGlobalBounds(GameObject gameObject, EBoundsType type)
        {
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

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                AddGameObjectToGlobalBounds(gameObject.transform.GetChild(i).gameObject, type);
            }
        }

        private static void AddBounds(Bounds bounds)
        {
            if (_globalBoundsIsFilled)
            {
                _globalBounds.Encapsulate(bounds);
            }
            else
            {
                _globalBounds = bounds;

                _globalBoundsIsFilled = true;
            }
        }
    }
}
