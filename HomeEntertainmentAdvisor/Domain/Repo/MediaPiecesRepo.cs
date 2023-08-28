﻿using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class MediaPiecesRepo : RepoBase<MediaPiece, Guid>, IMediaPiecesRepo
    {
        public MediaPiecesRepo(ApplicationDbContext context) : base(context)
        {
            dbSet=context.MediaPieces;
        }

        public override async Task<MediaPiece?> GetById(Guid id)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Guid> Save(MediaPiece entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity.Id;
        }
        public async Task<List<MediaPiece>> Search(string value)
        {
            if (string.IsNullOrEmpty(value))
                return await dbSet.Take(10).ToListAsync();
            var result = await dbSet.Where(x => x.Name.StartsWith(value)).ToListAsync();
            return result;
        }
    }
}
