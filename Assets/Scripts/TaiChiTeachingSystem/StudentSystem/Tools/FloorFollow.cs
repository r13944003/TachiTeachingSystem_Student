using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{

        // Make the floor follows the avatar move, including position and texture(grid)
        public class FloorFollow : MonoBehaviour
        {
            [SerializeField] private Transform _rightFootTransform;
            [SerializeField] private Transform _leftFootTransform;
            private Vector3 _targetPosition;
            private Transform _floor;
            private Material _gridMat;
            private Vector2 _initialOffset = new Vector2(0.3f, 0.4f);
            public float _distRatio = 0.22f;
            private void Start() {
                _floor = this.gameObject.transform;
                _gridMat = _floor.GetComponent<MeshRenderer>().material;
            }
            private void Update() {
                // Set Target Transform to the middle of the two foot
                _targetPosition = (_rightFootTransform.position + _leftFootTransform.position)/2;

                //地板會跟隨腳步
                _floor.position = new Vector3(_targetPosition.x, _floor.position.y, _targetPosition.z);

                // 網格(地毯)會保持不動，放便看出角色位移
                _gridMat.mainTextureOffset = _initialOffset + new Vector2(-_floor.localPosition.x, -_floor.localPosition.z) * _distRatio;
            }
        }
    }
}
