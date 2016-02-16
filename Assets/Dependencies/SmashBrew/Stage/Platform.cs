using System.Collections.Generic;
using UnityConstants;
using UnityEngine;

namespace HouraiTeahouse.SmashBrew {

    public abstract class TriggerStageElement : MonoBehaviour {

        private Collider[] _toIgnore;

        private void Awake() {
            List<Collider> colliders = new List<Collider>();
            foreach (Collider col in GetComponentsInChildren<Collider>()) {
                if (!col.isTrigger)
                    colliders.Add(col);
            }
            _toIgnore = colliders.ToArray();
        }

        protected void ChangeIgnore(Collider target, bool state) {
            if (target == null || !target.CompareTag(Tags.Player))
                return;

            foreach (Collider col in _toIgnore)
                Physics.IgnoreCollision(col, target, state);
        }

    }

    public class Platform : TriggerStageElement {

        private void OnTriggerEnter(Collider other) {
            ChangeIgnore(other, true);
        }

        private void OnTriggerExit(Collider other) {
            ChangeIgnore(other, false);
        }

    }

}
