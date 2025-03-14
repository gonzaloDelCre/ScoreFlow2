//using Domain.Entities.Matches;
//using Domain.Entities.MatchReferees;
//using Domain.Entities.Referees;
//using Infrastructure.Persistence.MatchReferees.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Persistence.MatchReferees.Mapper
//{
//    public class MatchRefereeMapper
//    {
//        public MatchRefereeEntity MapToEntity(MatchReferee matchReferee)
//        {
//            return new MatchRefereeEntity
//            {
//                MatchID = matchReferee.Match.MatchID,
//                RefereeID = matchReferee.Referee.RefereeID
//            };
//        }

//        public MatchReferee MapToDomain(MatchRefereeEntity entity, Match match, Referee referee)
//        {
//            return new MatchReferee(
//                match,
//                referee
//            );
//        }
//    }
//}
