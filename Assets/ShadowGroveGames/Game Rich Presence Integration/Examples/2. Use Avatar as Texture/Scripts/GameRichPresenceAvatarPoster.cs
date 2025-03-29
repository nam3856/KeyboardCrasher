using ShadowGroveGames.GameRichPresenceIntegration.Scripts.Presence;
using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Example
{
    public class GameRichPresenceAvatarPoster : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
        }

        public void OnGameRichPresenceReady(DiscordRPC.Message.ReadyMessage readyEvent)
        {
            Debug.Log("Received Ready!");
            readyEvent.User.GetAvatar(this, DiscordAvatarSize.x1024, (user, texture) =>
            {
                _meshRenderer.enabled = true;
                var renderer = GetComponent<Renderer>();
                renderer.material.mainTexture = texture;
            });
        }
    }
}