using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Beamable;
using Beamable.Common.Content;
using Beamable.Player;
using UnityEngine;

namespace Controllers
{
    public class PlayerStatsController : MonoBehaviour
    {
        // Cached beam context
        private BeamContext _beamContext;
    
        [Header("Debug")]
        [SerializeField] private string debugStatKey;
        [SerializeField] private string debugStatValue;

        private void Start()
        {
            _ = Setup();
        }
    
        private async Task Setup()
        {
            // Ensure that _beamContext is initialized by awaiting it.
            _beamContext = await BeamContext.Default.Instance;
            
            Debug.Log($"PlayerId = {_beamContext.PlayerId}");
            
            // Registering to the Stats Update to receive updates.
            _beamContext.Stats.OnDataUpdated += OnStatsChange;
        

        }

        private void OnStatsChange(SerializableDictionaryStringToSomething<PlayerStat> playerStats)
        {
            if(playerStats.Count == 0) return;
            StringBuilder stringBuilder = new StringBuilder("Stats Changed:");
            stringBuilder.AppendLine();
            foreach (var statIdAndValue in playerStats)
            {
                stringBuilder.AppendLine($"Stat: {statIdAndValue.Key} - Value: {statIdAndValue.Value.Value}");
            }
            Debug.Log(stringBuilder.ToString());
        }

        private async Task SetStat(string statKey, string value)
        {
            if (_beamContext.Stats.TryGetValue(statKey, out var statValue))
            {
                Debug.Log($"Stat {statKey} current value is: {statValue.Value}");
            }
            else
            {
                Debug.Log($"Stat {statKey} not found, creating it.");
            }
            await _beamContext.Stats.Set(statKey, value);
            var updatedStat = _beamContext.Stats[statKey];
            Debug.Log($"Stat {statKey} updated to {updatedStat.Value}");
        }

        private PlayerStat GetStat(string statKey)
        {
            if (_beamContext.Stats.TryGetValue(statKey, out var statValue))
            {
                return statValue;
            }
            throw new KeyNotFoundException($"Stat {statKey} not found");
        }
    
        [ContextMenu("Set Stat using Debug Value")]
        private void SetStatFromDebugValue()
        {
            _ = SetStat(debugStatKey, debugStatValue);
        }
    
        [ContextMenu("Get Stat using Debug Value")]
        private void GetStatFromDebugValue()
        {
            _ = GetStat(debugStatKey);
        }
    }
}
