// Copyright (c) MadDonkeySoftware

namespace Common.Model.Eve
{
    using System;
    using System.Collections.Generic;
    using Common.Model;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// A sample class
    /// </summary>
    public class UniverseType : MongoBase
    {
        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        [BsonElement("capacity")]
        public double Capacity { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [BsonElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the graphic id.
        /// </summary>
        [BsonElement("graphic_id")]
        public int GraphicId { get; set; }

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        [BsonElement("group_id")]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the icon id.
        /// </summary>
        [BsonElement("icon_id")]
        public int IconId { get; set; }

        /// <summary>
        /// Gets or sets the market group id.
        /// </summary>
        [BsonElement("market_group_id")]
        public int MarketGroupId { get; set; }

        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        [BsonElement("mass")]
        public double Mass { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the package volume.
        /// </summary>
        [BsonElement("package_volume")]
        public double PackageVolume { get; set; }

        /// <summary>
        /// Gets or sets the portion size.
        /// </summary>
        [BsonElement("portion_size")]
        public int PortionSize { get; set; }

        /// <summary>
        /// <c>True</c> if this is published; false otherwise.
        /// </summary>
        [BsonElement("published")]
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        [BsonElement("radius")]
        public double Radius { get; set; }

        /// <summary>
        /// Gets or sets the type id.
        /// </summary>
        [BsonElement("type_id")]
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        [BsonElement("volume")]
        public int Volume { get; set; }
    }
}