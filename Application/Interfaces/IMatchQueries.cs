﻿
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMatchQueries
    {
        Task<Match> GetById(int id);
        Task<IList<Match>> GetByUserId(int userId);
        Task<Match> GetByUsersIds(int userId1, int userId2);
        Task<IList<MatchResponse>> GetAllMatch();
        Task<bool> Exist(int userId1, int userId2);
        Task<IEnumerable<RankResponse>> ListTopMatchUsers();
    }
}
