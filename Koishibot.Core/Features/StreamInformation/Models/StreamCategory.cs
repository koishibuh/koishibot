﻿using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.StreamInformation.Models;

public class StreamCategory : IEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string TwitchId { get; set; } = string.Empty;

	// == ⚫ == //

	public async Task<StreamCategory> UpsertEntry(KoishibotDbContext database)
	{
		var result = await database.StreamCategories
			.FirstOrDefaultAsync(x => x.TwitchId == TwitchId);

		if (result == null)
		{
			await database.UpdateEntry(this);
		}

		return this;
	}
}

// == ⚫ == //

public class StreamCategoryConfig : IEntityTypeConfiguration<StreamCategory>
{
	public void Configure(EntityTypeBuilder<StreamCategory> builder)
	{
		builder.ToTable("StreamCategories");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Name);

		builder.HasIndex(p => p.Name)
			.IsUnique();

		builder.Property(p => p.TwitchId);
	}
}