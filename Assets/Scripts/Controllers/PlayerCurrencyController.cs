using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Beamable;
using Beamable.Common.Api.Inventory;
using Beamable.Common.Inventory;
using Beamable.Player;
using Beamable.Server.Clients;
using UnityEngine;

namespace Controllers
{
    public class PlayerCurrencyController : MonoBehaviour
    {
        // Cached beam context
        private BeamContext _beamContext;
        
        [Header("Debug")]
        [SerializeField] private long debugCurrencyValueToChange;
        [SerializeField] private CurrencyRef debugCurrencyReference;

        private void Start()
        {
            _ = Setup();
        }

        private async Task Setup()
        {
            // Ensure that BeamContext is initialized by awaiting it.
            _beamContext = await BeamContext.Default.Instance;
            
            
            // Refreshing the currencies to make sure that we will have the latest values.
            StringBuilder currenciesBuilder = new StringBuilder("Currencies:");
            currenciesBuilder.AppendLine();
            await _beamContext.Inventory.Currencies.Refresh();
            foreach (var inventoryCurrency in _beamContext.Inventory.Currencies)
            {
                currenciesBuilder.AppendLine($"Currency: {inventoryCurrency.CurrencyId} - Value: {inventoryCurrency.Amount}");
            }
            
            Debug.Log(currenciesBuilder.ToString());
            
            // Registering to the InventoryService to receive updates.
            _beamContext.Inventory.Currencies.OnDataUpdated += OnCurrenciesChanged;

        }

        private void UpdateCurrencyClientSide(string currencyId, long quantity)
        {
            // Create a Inventory Update Builder to add all inventory update operations. This can be used to update multiple items at once.
            InventoryUpdateBuilder inventoryUpdateBuilder = new InventoryUpdateBuilder();
            inventoryUpdateBuilder.CurrencyChange(currencyId, quantity);
            // Sending to BeamAPI to update the inventory.
            _beamContext.Inventory.Update(inventoryUpdateBuilder);
        }
    
        private void UpdateCurrencyMicroservice(string currencyId, long quantity)
        {
            // Get the client class for the CurrencyUpdater microservice.
            var currencyUpdaterClient = _beamContext.Microservices.GetClient<CurrencyUpdaterClient>();
            // Call the UpdateCurrency method on the CurrencyUpdater microservice. And Get the result to display it.
            currencyUpdaterClient.UpdateCurrency(currencyId, quantity).Then(result =>
            {
                Debug.Log($"Microservice Updated Currency {currencyId}. New Value: {result}");
            });
        }

        private void OnCurrenciesChanged(List<PlayerCurrency> changedCurrencies)
        {
            // Display the updated currencies.
            StringBuilder builder = new StringBuilder("Currencies Changed:");
            builder.AppendLine();
            foreach (var playerCurrency in changedCurrencies)
            {
                builder.AppendLine($"Currency: {playerCurrency.CurrencyId} - Value: {playerCurrency.Amount}");
            }
            Debug.Log(builder.ToString());
        }
        
        
        [ContextMenu("[Client] Update Currency from Debug Value")]
        public void UpdateCurrency_Client()
        {
            UpdateCurrencyClientSide(debugCurrencyReference.Id, debugCurrencyValueToChange);
        }
    
        [ContextMenu("[Microservice] Update Currency from Debug Value")]
        public void UpdateCurrency_Microservice()
        {
            UpdateCurrencyMicroservice(debugCurrencyReference.Id, debugCurrencyValueToChange);
        }
    
    }
}
