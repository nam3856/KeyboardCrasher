using ShadowGroveGames.GameRichPresenceIntegration.Scripts;
using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Examples
{
    public class UpdateRichPresenceOnRuntimeExample : MonoBehaviour
    {
        private float _interval = 2;
        private int _counter = 0;
        private bool _ready = false;

        public void OnReady()
        {
            _ready = true;
            GameRichPresenceManagerScript.Instance.UpdateDetails("In the main menu");
        }

        // Update is called once per frame
        void Update()
        {
            if (!_ready)
                return;

            if (_interval > 0)
            {
                _interval -= Time.deltaTime;
                return;
            }

            _interval = 2;
            _counter++;

            Debug.Log($"Updating Rich Presence to details to \"Count {_counter}\"");
            GameRichPresenceManagerScript.Instance.UpdateDetails("Count " + _counter);
        }
    }
}
