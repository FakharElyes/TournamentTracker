using System.ComponentModel;
using TrackerLibrary;
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
                        if (m.Winner == null || !uplayedOnlyCheckBox.Checked)
                        {
                            selectedMatchups.Add(m);
                        }                    
                    }
                }
            }

            if (selectedMatchups.Count > 0)
            {
                LoadMatchup(selectedMatchups.First());
            }

            DisplayMatchupInfo();
        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = selectedMatchups.Count > 0;

            teamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;

            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;

            versusLabel.Visible = isVisible;
            scoreButton.Visible = isVisible;
        }


        private void uplayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;
            double teamOneScore = 0;
            double teamTwoScore = 0;

            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        teamOneName.Text = m.Entries[0].TeamCompeting != null ? m.Entries[0].TeamCompeting.TeamName : teamNotSet;
                        if (double.TryParse(teamOneScoreValue.Text, out teamOneScore))
                        {
                            m.Entries[0].Score = teamOneScore; 
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid score for team 1.");
                            return;
                        }
                    }

                    if (i == 1)
                    {
                        teamTwoName.Text = m.Entries[1].TeamCompeting != null ? m.Entries[1].TeamCompeting.TeamName : teamNotSet;
                        if (double.TryParse(teamTwoScoreValue.Text, out teamTwoScore))
                        {
                            m.Entries[1].Score = teamTwoScore;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid score for team 2.");
                            return;
                        }
                    }

                }

                m.Winner = teamOneScore == teamTwoScore ? m.Entries[0].TeamCompeting : teamOneScore > teamTwoScore ? m.Entries[0].TeamCompeting : m.Entries[1].TeamCompeting;

                foreach (List<MatchupModel> round in tournamentModel.Rounds)
                {
                    foreach (MatchupModel rm in round)
                    {
                        foreach (MatchupEntryModel me in rm.Entries)
                        {
                            if (me.ParentMatchup != null)
                            {
                                if (me.ParentMatchup.Id == m.Id)
                                {
                                    me.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                } 
                            }
                        }
                    }
                }

                LoadMatchups((int)roundDropDown.SelectedItem);
                GlobalConfig.Connection.UpdateMatchup(m);
            }
        }
    }
}