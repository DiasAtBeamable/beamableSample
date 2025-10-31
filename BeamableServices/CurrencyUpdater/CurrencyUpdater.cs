using System.Threading.Tasks;
using Beamable.Common.Api.Inventory;
using Beamable.Server;

namespace Beamable.CurrencyUpdater
{
	[Microservice("CurrencyUpdater")]
	public partial class CurrencyUpdater : Microservice
	{
		[ClientCallable]
		public async Task<long> UpdateCurrency(string currencyId, long valueToChange)
		{
			InventoryUpdateBuilder inventoryUpdateBuilder = new InventoryUpdateBuilder();
			inventoryUpdateBuilder.CurrencyChange(currencyId, valueToChange);
			await Services.Inventory.Update(inventoryUpdateBuilder);
			return await Services.Inventory.GetCurrency(currencyId);
		}
	}
}
