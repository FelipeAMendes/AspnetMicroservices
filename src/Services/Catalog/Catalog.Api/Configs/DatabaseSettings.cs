namespace Catalog.Api.Configs
{
	public class DatabaseSettings : IDatabaseSettings
	{
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
		public string CollectionName { get; set; }
	}

	public interface IDatabaseSettings
	{
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
		string CollectionName { get; set; }
	}
}
