using System;
using System.Text;
using System.Threading.Tasks;
using Beamable;
using Beamable.Common.Content;
using UnityEngine;

namespace Controllers
{
    public class RemoteConfigController : MonoBehaviour
    {
        // Cached beam context
        private BeamContext _beamContext;
        
        // Reference to ActiveRemoteConfigContent
        [SerializeField]
        private ContentRef<ActiveRemoteConfigContent> _activeRemoteConfigContentRef;

        // Cached active GameConfigContent
        private GameConfigContent _activeGameConfig;

        private void Start()
        {
            _ = Setup();
        }

        private async Task Setup()
        {
            // Ensure that BeamContext is initialized by awaiting it.
            _beamContext = await BeamContext.Default.Instance;
            
            // Get the ActiveRemoteConfigContent reference and set the _activeGameConfig with the ActiveRemoveConfigContent value.
            var activeRemoteConfig = await _beamContext.Content.GetContent(_activeRemoteConfigContentRef);
            _activeGameConfig = await activeRemoteConfig.gameConfig.Resolve();
            
            // Subscribe to the ContentService to receive updates for content updates.
            _beamContext.Api.ContentService.Subscribe(OnContentChange);
        }

        private void OnContentChange(ClientManifest obj)
        {
            foreach (var clientContentInfo in obj.entries)
            {
                // Check if the ActiveRemoteConfigContent has changed to update the cached GameConfigContent.
                if (clientContentInfo.contentId == _activeRemoteConfigContentRef.Id)
                {
                    clientContentInfo.Resolve().Then(contentObject =>
                    {
                        var activeRemoteConfig = (ActiveRemoteConfigContent)contentObject;
                        activeRemoteConfig.gameConfig.Resolve().Then(gameConfigContent =>
                        {
                            StringBuilder builder = new StringBuilder("Active Game Config Changed");
                            builder.AppendLine();
                            builder.AppendLine($"Id: {gameConfigContent.Id}");
                            builder.AppendLine($"CurrencyPerScore: {gameConfigContent.CurrencyPerScore}");
                            builder.AppendLine($"ScoreMultiplier: {gameConfigContent.ScoreMultiplier}");
                            builder.AppendLine($"ScoreThresholdForBonus: {gameConfigContent.ScoreThresholdForBonus}");
                            Debug.Log(builder.ToString());
                            _activeGameConfig = gameConfigContent;
                        });
                    });
                }

                // Check if the current cached GameConfigContent has changed, so we ensure to have the latest values.
                if (clientContentInfo.contentId == _activeGameConfig.Id)
                {
                    clientContentInfo.Resolve().Then(contentObject =>
                    {
                        _activeGameConfig = (GameConfigContent)contentObject;
                        StringBuilder builder = new StringBuilder($"Active Game Config {_activeGameConfig.Id} had his values changed");
                        builder.AppendLine();
                        builder.AppendLine($"CurrencyPerScore: {_activeGameConfig.CurrencyPerScore}");
                        builder.AppendLine($"ScoreMultiplier: {_activeGameConfig.ScoreMultiplier}");
                        builder.AppendLine($"ScoreThresholdForBonus: {_activeGameConfig.ScoreThresholdForBonus}");
                    });
                }
            
            }
        }
    }
}
