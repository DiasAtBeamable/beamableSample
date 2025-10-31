using Beamable.Server;
using System.Threading.Tasks;

namespace Beamable.CurrencyUpdater
{
	public class Program
	{
		/// <summary>
		/// The entry point for the <see cref="CurrencyUpdater"/> service.
		/// </summary>
		public static async Task Main()
		{
			// inject data from the CLI.
			await MicroserviceBootstrapper.Prepare<CurrencyUpdater>();
			
			// run the Microservice code
			await MicroserviceBootstrapper.Start<CurrencyUpdater>();
		}
	}
}
