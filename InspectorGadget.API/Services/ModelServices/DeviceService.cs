﻿using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class DeviceService : IEntityService<Device, DeviceDto>
{
    public IDbRepository Repository { get; } = new DeviceRepository();
}