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
        private const string PrizesFile = "PrizeModels.csv";
        private const string PersonsFile = "PersonModels.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> people = PersonsFile.FullFilePath().LoadFile().ConvertToPersonModel();

            int CurrentId = people.Count == 0 ? 1 : people.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = CurrentId;
            people.Add(model);
            people.SaveToPeopleFile(PersonsFile);
            return model;


        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();

            int CurrentId = prizes.Count == 0 ? 1 : prizes.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = CurrentId;

            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            throw new NotImplementedException();
        }

        public List<PersonModel> GetPerson_All()
        {
            return PersonsFile.FullFilePath().LoadFile().ConvertToPersonModel();
        }
    }
}
