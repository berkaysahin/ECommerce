﻿using ECommerce.Services.Catalog.Interfaces;

namespace ECommerce.Services.Catalog.Settings;

public class DatabaseSettings : IDatabaseSettings
{
    public string CourseCollectionName { get; set; }

    public string CategoryCollectionName { get; set; }

    public string ConnectionString { get; set; }

    public string DatabaseName { get; set; }
}
