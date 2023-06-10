﻿using AutoMapper;
using Files.API.DataTransferObjects;
using Files.API.Exceptions;
using Files.API.Model;
using Files.API.Services.Contracts;
using MassTransit;
using MassTransitModels.File;
using Microsoft.EntityFrameworkCore;
using File = Files.API.Model.File;

namespace Files.API.Services;

public class FileService : IFileService
{
    private readonly ApplicationDatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public FileService(
        ApplicationDatabaseContext context,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<Model.File> GetFileAsync(Guid userId, Guid fileId)
    {
        var file = await _context.Files
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == fileId);
        
        if (file is null)
        {
            throw new InvalidFileIdBadRequest(fileId);
        }

        return file;
    }
    public async Task<ICollection<Model.File>> GetFilesByUserIdAsync(Guid userId)
    {
        var files = await _context.Files
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return files;
    }
    public async Task<File> CreateFileAsync(CreateFileDto dto)
    {
        var entity = _mapper.Map<File>(dto);

        _context.Files.Add(entity);

        await _context.SaveChangesAsync();

        return entity;
    }
    public async Task UpdateFileAsync(Guid userId, UpdateFileDto dto)
    {
        var file = await _context.Files
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (file is null)
        {
            throw new InvalidFileIdBadRequest(dto.Id);
        }

        _mapper.Map(dto, file);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFileAsync(Guid userId, Guid fileId)
    {
        var file = await _context.Files.FirstOrDefaultAsync(
            x => x.UserId == userId && x.Id == fileId);

        if (file == null)
        {
            throw new InvalidFileIdBadRequest(fileId);
        }

        await _publishEndpoint.Publish<FileDeleted>(new
        {
            Name = file.Id.ToString(),
            Extension = file.Extension
        });
        
        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
    }
    public async Task UploadFileAsync(FormFileDto dto, Guid userId)
    {
        var createFileDto = _mapper.Map<CreateFileDto>(dto);
        createFileDto.UserId = userId;
        var file = await CreateFileAsync(createFileDto);
        await using var stream = new MemoryStream();
        await dto.File.CopyToAsync(stream);
        var bytes = stream.ToArray();
        await _publishEndpoint.Publish<FileCreated>(new
        {
            Bytes = bytes,
            Name = file.Id.ToString(),
            Extension = file.Extension,
        });
    }
}