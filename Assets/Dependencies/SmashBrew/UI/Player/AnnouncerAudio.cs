using UnityEngine;
using HouraiTeahouse.SmashBrew.UI;
using UnityEngine.EventSystems;

namespace HouraiTeahouse.SmashBrew {

    public class AnnouncerAudio : CharacterUIComponent<AudioSource>, ISubmitHandler {

        public void OnSubmit(BaseEventData eventData) {
            if(Component)
                Component.Play();
        }

        public override void SetData(CharacterData data) {
            base.SetData(data);
            if (Component == null || data == null)
                return;
            Component.clip = data.Announcer.Load();
        }
    }
}
