﻿
namespace Files.API.DataTransferObjects.Directory;

public class DirectoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public Guid? ParentDirectoryId { get; set; }
    public ICollection<Guid> Directories { get; set; }
    public ICollection<Guid> Files { get; set; }
}