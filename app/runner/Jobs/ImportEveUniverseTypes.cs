// Copyright (c) MadDonkeySoftware

namespace Runner.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;

    using Common.Data;
    using Common.Model;
    using Common.Model.Eve;
    using Common.Model.JobData;
    using MongoDB.Driver;
    using Newtonsoft.Json;

    /// <summary>
    /// Job for creating the mongo indexes
    /// </summary>
    public class ImportEveUniverseTypes : EveDataJob
    {
        private static readonly SmartHttpClient Client = new SmartHttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportEveUniverseTypes"/> class.
        /// </summary>
        /// <param name="jobUuid">The job uuid this is running for.</param>
        /// <param name="dbFactory">The dbFactory for this job to use.</param>
       public ImportEveUniverseTypes(string jobUuid, IDbFactory dbFactory)
            : base(jobUuid, dbFactory)
        {
        }

        /// <summary>
        /// The main body for the job being run.
        /// </summary>
        protected override void Work()
        {
            this.AddMessage("Starting universe item type import.");
            this.ImportEveUniverseTypeData();
            this.AddMessage("Finished universe item type import.");
        }

        private void ImportEveUniverseTypeData()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            var typeCol = this.DbFactory.GetCollection<UniverseType>(CollectionNames.EveUniverseTypes);

            if (typeCol.Count(x => true) > 0)
            {
                this.AddMessage("Skipping job run since data already exists in the system.");
                return;
            }

            var page = 1;
            var orderCount = 0;
            var typeIds = new List<int>();

            this.AddMessage("Finding all available types in the eve universe.");
            do
            {
                var uri = this.CreateEndpoint(string.Format(
                    CultureInfo.InvariantCulture, "/universe/types?page={0}", page));
                var result = Client.GetWithReties(uri);
                var types = JsonConvert.DeserializeObject<List<int>>(result);

                typeIds.AddRange(types);

                orderCount = types.Count();
                page++;
            }
            while (orderCount == 1000);

            // Next get the actual data for each market type
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            foreach (var typeId in typeIds)
            {
                this.AddMessage("Finding information on type {0}.", typeId);
                var uri = this.CreateEndpoint(string.Format(CultureInfo.InvariantCulture, "/universe/types/{0}", typeId));
                var result = Client.GetWithReties(uri);
                var details = JsonConvert.DeserializeObject<EveUniverseType>(result, jsonSerializerSettings);

                var filterCondition = Builders<UniverseType>.Filter.Eq(r => r.TypeId, details.TypeId);
                var updateCondition = Builders<UniverseType>.Update.Set(r => r.TypeId, details.TypeId)
                                                                   .Set(r => r.Capacity, details.Capacity)
                                                                   .Set(r => r.Description, details.Description)
                                                                   .Set(r => r.GraphicId, details.GraphicId)
                                                                   .Set(r => r.GroupId, details.GroupId)
                                                                   .Set(r => r.IconId, details.IconId)
                                                                   .Set(r => r.MarketGroupId, details.MarketGroupId)
                                                                   .Set(r => r.Mass, details.Mass)
                                                                   .Set(r => r.Name, details.Name)
                                                                   .Set(r => r.PackageVolume, details.PackagedVolume)
                                                                   .Set(r => r.PortionSize, details.PortionSize)
                                                                   .Set(r => r.Published, details.Published)
                                                                   .Set(r => r.Radius, details.Radius)
                                                                   .Set(r => r.Volume, details.Volume);

                typeCol.UpdateOne(filterCondition, updateCondition, new UpdateOptions { IsUpsert = true });
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification="Used by Newtonsoft.Json")]
        internal class EveUniverseType
        {
            [JsonProperty("capacity")]
            internal double Capacity { get; set; }

            [JsonProperty("description")]
            internal string Description { get; set; }

            [JsonProperty("graphic_id")]
            internal int GraphicId { get; set; }

            [JsonProperty("group_id")]
            internal int GroupId { get; set; }

            [JsonProperty("icon_id")]
            internal int IconId { get; set; }

            [JsonProperty("market_group_id")]
            internal int MarketGroupId { get; set; }

            [JsonProperty("mass")]
            internal double Mass { get; set; }

            [JsonProperty("name")]
            internal string Name { get; set; }

            [JsonProperty("packaged_volume")]
            internal double PackagedVolume { get; set; }

            [JsonProperty("portion_size")]
            internal double PortionSize { get; set; }

            [JsonProperty("published")]
            internal bool Published { get; set; }

            [JsonProperty("radius")]
            internal double Radius { get; set; }

            [JsonProperty("type_id")]
            internal int TypeId { get; set; }

            [JsonProperty("volume")]
            internal double Volume { get; set; }
        }
    }
}