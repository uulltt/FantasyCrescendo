using UnityEngine;

namespace HouraiTeahouse.SmashBrew {

    /// <summary> An AnimationBehaviour that causes Characters to constantly move while in the state </summary>
    public class Move : BaseAnimationBehaviour<Character> {

        [SerializeField]
        float _acceleration = 2.2f;

        [SerializeField]
        float _capSpeed = 3.2f;

        [SerializeField]
        [ReadOnly]
        float _currentSpeed;

        [SerializeField]
        [Tooltip("Whether movement in the state ignores or adheres to the difference in state speed")]
        bool _ignoreStateSpeed;

        [SerializeField]
        [Tooltip("The base movement speed that the character will move at while in the state")]
        float _initialSpeed = 2f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (!Target)
                return;
            _currentSpeed = _initialSpeed;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (!Target)
                return;
            Target.Move(_currentSpeed);
            _currentSpeed += _acceleration * Target.FixedDeltaTime;
            if ((_initialSpeed < _capSpeed && _currentSpeed > _capSpeed)
                || (_initialSpeed > _capSpeed && _currentSpeed < _capSpeed))
                _currentSpeed = _capSpeed;
        }

    }

}