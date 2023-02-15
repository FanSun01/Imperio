﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProtobufAny.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufAny.Database.Configurations
{
    public class AlbumMap : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("Album");
            builder.HasKey(x => x.AlbumId);
            builder.Property(x => x.AlbumId).HasColumnName("AlbumId");
            builder.Property(x => x.Title).HasColumnName("Title");
            builder.Property(x => x.ArtistId).HasColumnName("ArtistId");
        }
    }
}
