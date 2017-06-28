using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace MapEditor
{
    [ExecuteInEditMode]
    public class EditModeGridSnap : MonoBehaviour
    {
        public bool ActiveSnap = true;

        public EBoundsType BoundsType;

        [SerializeField] private Vector2 _offset;

        public void Fit(Bounds globalBounds)
        {
            Vector2 newPosition = new Vector2();

            float snapValue = MapEditorModel.instance.caseSize;
            float snapInverse = 1/snapValue;

            // Compute distance between closest culumn and the min.x
            float closestCulumn = Mathf.Floor(globalBounds.min.x*snapInverse)*snapValue;

            // Compute the case count encapsulate by the globalbounds
            float encapsulateCaseCount_width = Mathf.Ceil(globalBounds.size.x*snapInverse);

            // Compute offset to the closest culum
            float offset_x = (encapsulateCaseCount_width*snapValue - globalBounds.size.x)/2;

            newPosition.x = closestCulumn + offset_x + globalBounds.size.x/2;

            // Compute distance between closest culumn line and the min.x
            float closestLine = Mathf.Floor(globalBounds.min.y*snapInverse)*snapValue;

            // Compute the case count encapsulate by the globalbounds
            float encapsulateCaseCount_height = Mathf.Ceil(globalBounds.size.y*snapInverse);

            // Compute offset to the closest culum line
            float offset_y = (encapsulateCaseCount_height*snapValue - globalBounds.size.y)/2;

            newPosition.y = closestLine + offset_y + globalBounds.size.y/2;

            Vector2 transformOffset = globalBounds.center - transform.position;

            transform.position = newPosition - transformOffset;

            SaveOffset();
        }

        public void SaveOffset()
        {
            Vector2i? coord = MapEditorModel.instance.grid.GetCoordAt(transform.position);

            if (coord.HasValue)
            {
                Vector2 casePosition = MapEditorModel.instance.grid.GetCasePosition(coord.Value);

                _offset = new Vector2(transform.position.x, transform.position.y) - casePosition;
            }
        }

        void Update()
        {
            if (ActiveSnap)
            {
                float snapValue = MapEditorModel.instance.caseSize;

                if (snapValue == 0)
                {
                    return;
                }

                Vector2i? coord = MapEditorModel.instance.grid.GetCoordAt(transform.position);

                if (coord.HasValue)
                {
                    Vector2 casePosition = MapEditorModel.instance.grid.GetCasePosition(coord.Value);

                    transform.position = casePosition + _offset;
                }

                //float snapInverse = 1/snapValue;

                //float x, y, z;

                //x = Mathf.Floor(transform.position.x * snapInverse) * snapValue + _offset.x;
                //y = Mathf.Floor(transform.position.y * snapInverse) * snapValue + _offset.y;
                //z = transform.position.z;

                ////// if snapValue = .5, x = 1.45 -> snapInverse = 2 -> x*2 => 2.90 -> round 2.90 => 3 -> 3/2 => 1.5
                ////// so 1.45 to nearest .5 is 1.5
                ////x = Mathf.Round(transform.position.x*snapInverse)/snapInverse;
                ////y = Mathf.Round(transform.position.y*snapInverse)/snapInverse;
                ////z = Depth; // depth from camera

                //transform.position = new Vector3(x, y, z);
            }
        }
    }
}