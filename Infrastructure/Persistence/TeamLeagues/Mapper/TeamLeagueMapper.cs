//using Domain.Entities.Leagues;
//using Domain.Entities.TeamLeagues;
//using Domain.Entities.Teams;
//using Infrastructure.Persistence.TeamLeagues.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Persistence.TeamLeagues.Mapper
//{
//    public class TeamLeagueMapper
//    {
//        public TeamLeagueEntity MapToEntity(TeamLeague teamLeague)
//        {
//            return new TeamLeagueEntity
//            {
//                TeamID = teamLeague.Team.TeamID,
//                LeagueID = teamLeague.League.LeagueID,
//                JoinDate = teamLeague.JoinDate
//            };
//        }

//        public TeamLeague MapToDomain(TeamLeagueEntity entity, Team team, League league)
//        {
//            return new TeamLeague(
//                team,
//                league,
//                entity.JoinDate
//            );
//        }
//    }
//}
