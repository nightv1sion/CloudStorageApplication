﻿namespace MassTransitModels.File;

public interface FileDeleted
{
    public string Name { get; }
    public string Extension { get; }
}