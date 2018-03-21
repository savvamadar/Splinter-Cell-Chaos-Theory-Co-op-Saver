using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace SplinterCellCTCOOPSaver
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        public Form1()
        {
            InitializeComponent();
        }

        bool isHost = false;
        bool isValidSaveState = false;

        string hostFile = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Ubisoft\Tom Clancy's Splinter Cell Chaos Theory\CacheCoopQuickSaveHost.sav";
        string clientFile = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Ubisoft\Tom Clancy's Splinter Cell Chaos Theory\CacheCoopQuickSaveClient.sav";

        string hostAlteredNameFile = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Ubisoft\Tom Clancy's Splinter Cell Chaos Theory\host.sav";
        string clientAlteredNameFile = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Ubisoft\Tom Clancy's Splinter Cell Chaos Theory\client.sav";

        Thread trd;

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(hostAlteredNameFile))
            {
                if (File.Exists(hostFile))
                {
                    File.Delete(hostFile);
                }
                File.Copy(hostAlteredNameFile, hostFile);
                File.Delete(hostAlteredNameFile);
            }
            else if (File.Exists(clientAlteredNameFile))
            {
                if (File.Exists(clientFile))
                {
                    File.Delete(clientFile);
                }
                File.Copy(clientAlteredNameFile, clientFile);
                File.Delete(clientAlteredNameFile);
            }
            if (File.Exists(hostFile))
            {
                isHost = true;
                label1.Text = "Found Host save!";
                isValidSaveState = true;
            }
            else if (File.Exists(clientFile))
            {
                label1.Text = "Found Client save!";
                isValidSaveState = true;
            }
            else
            {
                label1.Text = "No Default Save Found";
            }
            if (File.Exists(hostAlteredNameFile))
            {
                isHost = true;
                label2.Text = "Found OLD Host save!";
                isValidSaveState = true;
            }
            else if (File.Exists(clientAlteredNameFile))
            {
                label2.Text = "Found OLD Client save!";
                isValidSaveState = true;
            }
            else
            {
                label2.Text = "No Special Save Found";
                if(isHost && File.Exists(hostFile))
                {
                    label3.Text = "Making a copy...";
                    try
                    {
                        File.Copy(hostFile, hostAlteredNameFile);
                        label3.Text = "Made a copy of save file for Host.";
                    }
                    catch (Exception ex)
                    {
                        isValidSaveState = false;
                        label3.Text = "Failed to make a copy of the save file, try running me as an admin!";
                    }
                }
                else if (!isHost && File.Exists(clientFile))
                {
                    label3.Text = "Making a copy...";
                    try
                    {
                        File.Copy(clientFile, clientAlteredNameFile);
                        label3.Text = "Made a copy of save file for Client.";
                    }
                    catch(Exception ex)
                    {
                        isValidSaveState = false;
                        label3.Text = "Failed to make a copy of the save file, try running me as an admin!";
                    }
                    
                }
                else
                {
                    label3.Text = "You have to have already Quick Saved.";
                }
            }
            if (isValidSaveState)
            {
                label4.Text = "Press F10 AFTER quick saving in SP:CT, after you hear the DING quick load";
                trd = new Thread(getKeyPress);
                trd.IsBackground = true;
                trd.Start();
            }
            else
            {
                label4.Text = "You have to have a quick save to restore it.";
            }

        }

        private void getKeyPress()
        {
            while (true)
            {
                if (isValidSaveState)
                {
                    if ((GetAsyncKeyState((int)(Keys.F10)) & 0x8000) > 0)
                    {
                        try
                        {
                            if (isHost)
                            {
                                if (File.Exists(hostFile))
                                {
                                    File.Delete(hostFile);
                                }
                                File.Copy(hostAlteredNameFile, hostFile);
                                File.Delete(hostAlteredNameFile);
                            }
                            else
                            {
                                if (File.Exists(clientFile))
                                {
                                    File.Delete(clientFile);
                                }
                                File.Copy(clientAlteredNameFile, clientFile);
                                File.Delete(clientAlteredNameFile);
                            }
                            SoundPlayer audio = new SoundPlayer(SplinterCellCTCOOPSaver.Properties.Resources.Ding);
                            audio.Play();
                            Thread.Sleep(500);
                            Application.Exit();

                        }
                        catch(Exception ex)
                        {

                        }
                        //replace
                        //delete
                        //playSound
                        //close
                    }
                    Thread.Sleep(10);
                }
            }
        }
    }
}
