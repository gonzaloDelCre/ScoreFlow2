using Domain.Entities.Players;
using Domain.Entities.Matches;
using Domain.Shared;

namespace Domain.Entities.PlayerStatistics
{
    public class PlayerStatistic
    {
        public PlayerStatisticID PlayerStatisticID { get; private set; }
        public MatchID MatchID { get; private set; }
        public PlayerID PlayerID { get; private set; }
        public Goals Goals { get; private set; }
        public Assists Assists { get; private set; }
        public YellowCards YellowCards { get; private set; }
        public RedCards RedCards { get; private set; }
        public int? MinutesPlayed { get; set; }
        public DateTime CreatedAt { get; private set; }

        public Match Match { get; private set; }
        public Player Player { get; private set; }

        public PlayerStatistic(PlayerStatisticID playerStatisticID, MatchID matchID, PlayerID playerID, Goals goals, Assists assists, YellowCards yellowCards, RedCards redCards, Match match, Player player, DateTime createdAt, int? minutesPlayed = null)
        {
            PlayerStatisticID = playerStatisticID ?? throw new ArgumentNullException(nameof(playerStatisticID));
            MatchID = matchID ?? throw new ArgumentNullException(nameof(matchID));
            PlayerID = playerID ?? throw new ArgumentNullException(nameof(playerID));
            Goals = goals ?? throw new ArgumentNullException(nameof(goals));
            Assists = assists ?? throw new ArgumentNullException(nameof(assists));
            YellowCards = yellowCards ?? throw new ArgumentNullException(nameof(yellowCards));
            RedCards = redCards ?? throw new ArgumentNullException(nameof(redCards));
            Match = match ?? throw new ArgumentNullException(nameof(match));
            Player = player ?? throw new ArgumentNullException(nameof(player));
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
            MinutesPlayed = minutesPlayed;
        }
        public void Update(Goals goals, Assists assists, YellowCards yellowCards, RedCards redCards, int minutesPlayed)
        {
            Goals = goals;
            Assists = assists;
            YellowCards = yellowCards;
            RedCards = redCards;
            MinutesPlayed = minutesPlayed;
        }

        public PlayerStatistic(PlayerStatisticID playerStatisticID, PlayerID playerID, MatchID matchID, Goals goals, Assists assists, YellowCards yellowCards, RedCards redCards, DateTime createdAt, int? minutesPlayed)
        {
            PlayerStatisticID = playerStatisticID;
            PlayerID = playerID;
            MatchID = matchID;
            Goals = goals;
            Assists = assists;
            YellowCards = yellowCards;
            RedCards = redCards;
            CreatedAt = createdAt;
            MinutesPlayed = minutesPlayed;
        }

        public void UpdateMinutesPlayed(int? minutes)
        {
            MinutesPlayed = minutes;
        }

    }

}
