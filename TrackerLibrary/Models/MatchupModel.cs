using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        private readonly string notDetermined = "Matchup Not Yet Determined";

        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// the ID from the database that will be used to identify the winner
        /// </summary>
        public int WinnerId { get; set; }

        public TeamModel Winner { get; set; }
        public int MatchupRound { get; set; }


        public string DisplayName
        {
            get
            {
                string output = string.Empty;

                foreach (MatchupEntryModel me in Entries)
                {
                    if (me.TeamCompeting != null)
                    {
                        if (string.IsNullOrEmpty(output))
                        {
                            output = me.TeamCompeting.TeamName;
                        }
                        else
                        {
                            output += $" vs. {me.TeamCompeting.TeamName}";
                        } 
                    }
                    else
                    {
                        output = notDetermined;
                        break;
                    }
                }

                return output;
            }
        }
    }
}
