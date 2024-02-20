using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace Infrastructure.Queries
{
    public class MatchQueries : IMatchQueries
    {
        private readonly AppDbContext _context;

        public MatchQueries(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Exist(int userId1, int userId2)
        {
            bool exist = await _context.Matches
                .AnyAsync(el => 
                                (el.User1Id == userId1 && el.User2Id == userId2) ||
                                (el.User1Id == userId2 && el.User2Id == userId1));
            return exist;
        }

        public async Task<IList<MatchResponse>> GetAllMatch()
        {
            IList<MatchResponse> matches = await _context.Matches
                .Select(m => new MatchResponse
                {
                    Id = m.MatchId,
                    User1 = m.User1Id, 
                    User2 = m.User2Id,
                })
                .ToListAsync();
            return matches;
        }

        public async Task<Match> GetById(int id)
        {
            Match match = await _context.Matches.FirstOrDefaultAsync(x => x.MatchId == id);

            return match;
        }

        public async Task<IList<Match>> GetByUserId(int userId)
        {
            IList<Match> matches = await _context.Matches.Where(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
            return matches;
        }

        public async Task<Match> GetByUsersIds(int userId1, int userId2)
        {

            Match match = await _context.Matches
                                        .Where(x => (x.User1Id == userId1 && x.User2Id == userId2) || (x.User1Id == userId2 && x.User2Id == userId1))
                                        .FirstOrDefaultAsync();

            return match;
        }

        public async Task<IEnumerable<RankResponse>> ListTopMatchUsers()
        {
            try
            { 
                List<RankResponse> rankList = new List<RankResponse>();

                var ranking1 = await _context.Matches
                                    .GroupBy(x => x.User1Id)
                                    .Select(g => new RankResponse
                                    {
                                        UserId = g.Key,
                                        MatchQty = g.Count()
                                    })
                                    .OrderByDescending(x => x.MatchQty)
                                    .Take(10)
                                    .ToListAsync();

                var ranking2 = await _context.Matches
                                        .GroupBy(x => x.User2Id)
                                        .Select(g => new RankResponse
                                        {
                                            UserId = g.Key,
                                            MatchQty = g.Count()
                                        })
                                        .OrderByDescending(x => x.MatchQty)
                                        .Take(10)
                                        .ToListAsync();

                rankList.AddRange(ranking1);

                foreach(var e in ranking2)
                {
                    var x = rankList.FirstOrDefault(x => x.UserId == e.UserId);
                    if (x != null)
                    {
                        var aux = new RankResponse
                        {
                            UserId = e.UserId,
                            MatchQty = e.MatchQty + x.MatchQty
                        };
                        rankList.Remove(x);
                        rankList.Add(aux);
                    }
                    else
                    {
                        rankList.Add(e);
                    }
                }

                var response = rankList.OrderByDescending(x => x.MatchQty).Take(10);

                return response;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        } 
    }
}
