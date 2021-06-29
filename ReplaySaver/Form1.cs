using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ReplaySaver
{
    public partial class ReplaySaverForm : Form
    {
        bool _running = false;
        public ReplaySaverForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Characters_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void SetStatusText(string text)
        {
            runningLabel.Invoke((MethodInvoker)delegate { runningLabel.Text = text; });
        }

        private const int MaxLevelNumber = 1035;
        public static string MakeMapString(int levelNumber)
        {
            var numberForm = "";

            if (levelNumber < 0 || levelNumber > MaxLevelNumber)
            {
                return "Invalid-map-number";
            }
            if (levelNumber < 100)
            {
                // Map number is expressed with two digits
                numberForm = levelNumber.ToString("D2");
            }
            else
            {
                // Map number is expressed with a leading letter and a digit/letter
                var firstChar = (char)((levelNumber - 100) / 36 + 'A');
                var remainder = (levelNumber - 100) % 36;
                var secondChar = remainder < 10 ? (char)(remainder + '0') : (char)(remainder - 10 + 'A');
                char[] chars = { firstChar, secondChar };
                numberForm = new string(chars);
            }
            return "MAP" + numberForm;
        }

        private void TryToSaveReplay(int mapNum, string character, string replayfolder)
        {
            string mapName = MakeMapString(mapNum);

            if (!Directory.Exists(replayfolder))
            {
                SetStatusText("Couldn't find your imaginary folder " + replayfolder);
                return;
            }

            try
            {
                Directory.SetCurrentDirectory(replayfolder);
                string characterName = character.Trim().ToLower().Replace(" ", "");

                List<string> characters;
                if (characterName == "all")
                {
                    characters = new List<string>(new string[] { "sonic", "tails", "knuckles", "amy", "fang", "metalsonic"});
                }
                else
                {
                    characters = new List<string>(new string[] { characterName });
                }
                foreach (string characterString in characters)
                {

                    string replayName = mapName + "-" + characterString + "-last.lmp";
                    if (File.Exists(replayName))
                    {
                        for (int i = 0; i < 10000; i++)
                        {
                            string newName = "guests/" + mapName + "-" + characterString + "-guest-" + i.ToString("D4") + ".lmp";

                            if (!File.Exists(newName))
                            {
                                File.Move(replayName, newName);
                                SetStatusText("Saved a new guest to " + newName);
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                SetStatusText("Saving the replay failed. Trying again in a sec...");
            }
        }

        private void RunLoop()
        {
            int count = 0;
            while (_running)
            {
                count++;
                int map = 0;
                string character = "";
                string folder = "";
                mapUpDown.Invoke((MethodInvoker)delegate { map = (int)mapUpDown.Value; });
                characterBox.Invoke((MethodInvoker)delegate { character = characterBox.Text; });
                replayFolderBox.Invoke((MethodInvoker)delegate { folder = replayFolderBox.Text; });

                TryToSaveReplay(map, character, folder);
                Thread.Sleep(5000);
            }
            Thread.CurrentThread.Abort();
        }
        
        private void runButton_Click(object sender, EventArgs e)
        {
            _running = !_running;

            if (_running)
            {
                runButton.Text = "Stop";
                runningLabel.Text = "Running! Go blast some robos!";
                ThreadStart runStart = new ThreadStart(RunLoop);
                Thread runThread = new Thread(runStart);
                runThread.Start();
            }
            else
            {
                runButton.Text = "Run";
                runningLabel.Text = "Not running";
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
