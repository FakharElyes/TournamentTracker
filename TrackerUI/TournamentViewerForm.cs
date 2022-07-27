using System.ComponentModel;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private readonly string teamNotSet = "Not Yet Set";
        private TournamentModel tournamentModel;
        BindingList<int> rounds = new();
        BindingList<MatchupModel> selectedMatchups = new();

        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();
            this.tournamentModel = tournamentModel;

            WireUpLists();

            LoadFormData();
            LoadRounds();
        }

        private void LoadMatchup(MatchupModel m)
        {
            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        teamOneName.Text = m.Entries[0].TeamCompeting != null ? m.Entries[0].TeamCompeting.TeamName : teamNotSet;
                        teamOneScoreValue.Text = m.Entries[0].TeamCompeting != null ? m.Entries[0].Score.ToString() : String.Empty;

                        teamTwoName.Text = teamNotSet;
                        teamTwoScoreValue.Text = String.Empty;
                    }

                    if (i == 1)
                    {
                        teamTwoName.Text = m.Entries[1].TeamCompeting != null ? m.Entries[1].TeamCompeting.TeamName : teamNotSet;
                        teamTwoScoreValue.Text = m.Entries[1].TeamCompeting != null ? m.Entries[1].Score.ToString() : String.Empty;
                    }

                } 
            }
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
        }

        private void LoadFormData()
        {
            tournamentName.Text = this.tournamentModel.TournamentName;
        }

        private void WireUpLists()
        {
            roundDropDown.DataSource = rounds;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }


        private void LoadRounds()
        {
            rounds.Clear();

            rounds.Add(1);
            int currRound = 1;

            foreach (List<MatchupModel> matchup in tournamentModel.Rounds)
            {
                if (matchup.Count > 0 && matchup.First().MatchupRound > currRound)
                {
                    currRound = matchup.First().MatchupRound;
                    rounds.Add(currRound);
                }
            }

            LoadMatchups(1);
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);
        }

        private void LoadMatchups(int round)
        {
            foreach (List<MatchupModel> matchups in tournamentModel.Rounds)
            {
                if (matchups.Count > 0 && matchups.First().MatchupRound == round)
                {
                    selectedMatchups.Clear();
                    foreach (MatchupModel m in matchups)
                    {
                        selectedMatchups.Add(m);
                    }
                }
            }

            MatchupModel matchupModel = selectedMatchups.Count > 0 ? selectedMatchups.First() : new MatchupModel() ;


            LoadMatchup(matchupModel);
        }
    }
}