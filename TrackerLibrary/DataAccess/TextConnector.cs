using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;


namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        public void CreatePerson(PersonModel model)
        {
            List<PersonModel> people = GlobalConfig.PersonsFile.FullFilePath().LoadFile().ConvertToPersonModel();

            int CurrentId = people.Count == 0 ? 1 : people.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = CurrentId;
            people.Add(model);
            people.SaveToPeopleFile();
        }

        public void CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();

            int CurrentId = prizes.Count == 0 ? 1 : prizes.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = CurrentId;

            prizes.Add(model);

            prizes.SaveToPrizeFile();
        }

        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamsFile.FullFilePath().LoadFile().ConvertToTeeamModel();
            int CurrentId = teams.Count == 0 ? 1 : teams.OrderByDescending(x => x.Id).First().Id + 1;
            model.Id = CurrentId;
            teams.Add(model);
            teams.SaveToTeamFile();
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModel();

            int CurrentId = tournaments.Count == 0 ? 1 : tournaments.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = CurrentId;

            model.SaveRoundsToFile();

            tournaments.Add(model);

            tournaments.SaveToTournamentsFile();
        }

        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PersonsFile.FullFilePath().LoadFile().ConvertToPersonModel();
        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamsFile.FullFilePath().LoadFile().ConvertToTeeamModel();
        }

        public List<TournamentModel> GetTournament_All()
        {
            return GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModel();

        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchupToFile();
        }
    }
}
