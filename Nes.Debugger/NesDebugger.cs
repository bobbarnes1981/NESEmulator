using Nes.Library;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Nes.Debugger
{
    public partial class NesDebugger : Form
    {
        Computer m_nes;

        private bool m_running = false;

        public NesDebugger()
        {
            InitializeComponent();

            if (File.Exists("nes.log"))
            {
                File.Delete("nes.log");
            }
            File.Create("nes.log");

            string src_file = "..\\..\\..\\..\\NES Test (USA).nes";

            NesFile file = new NesFile(src_file);
            
            m_nes = new Computer(file, log);
            update();

            textBoxDisassembly.Text = m_nes.Cpu.Disassemble(0x8000);
        }

        private void log(string message, params object[] args)
        {
            if (InvokeRequired)
            {
                //Invoke((Action)delegate () { log(message, args); });
            }
            else
            {
                //File.AppendAllText("nes.log", string.Format(message + "\r\n", args));

                textBoxLog.AppendText(string.Format(message, args));
                textBoxLog.AppendText("\r\n");
            }
        }

        private void update()
        {
            if (InvokeRequired)
            {
                Invoke((Action)delegate () { update(); });
            }
            else
            {
                textBoxRegA.Text = "0x" + m_nes.Cpu.RegA.ToString("X2");
                textBoxRegP.Text = "0x" + m_nes.Cpu.RegP.ToString("X2");
                textBoxRegX.Text = "0x" + m_nes.Cpu.RegX.ToString("X2");
                textBoxRegY.Text = "0x" + m_nes.Cpu.RegY.ToString("X2");
                textBoxPC.Text = "0x" + m_nes.Cpu.PC.ToString("X4");
                textBoxSP.Text = "0x01" + m_nes.Cpu.SP.ToString("X2");

                textBoxScan.Text = m_nes.Ppu.Scanline.ToString();
                textBoxPix.Text = m_nes.Ppu.Pixel.ToString();
                textBoxStatus.Text = "0x" + m_nes.Ppu.RegStat.ToString("X2");
                //textBoxCtrl.Text = "0x" + m_nes.Ppu.RegCtrl.ToString("X2");
                //textBoxMask.Text = "0x" + m_nes.Ppu.RegMask.ToString("X2");
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            m_running = true;
            buttonGo.Enabled = false;
            buttonStep.Enabled = false;
            buttonStop.Enabled = true;

            new Thread(() =>
            {
                try
                {
                    while (m_running)
                    {
                        m_nes.Step();
                        //update(); 
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                update();
                stop();
            }).Start();
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            buttonGo.Enabled = false;
            buttonStep.Enabled = false;
            buttonStop.Enabled = true;

            new Thread(() =>
            {
                try
                {
                    m_nes.Step();
                    //update();
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                update();
                stop();
            }).Start();
        }

        private void stop()
        {
            if (InvokeRequired)
            {
                Invoke((Action)delegate(){ stop(); });
            }
            else
            {
                buttonGo.Enabled = true;
                buttonStep.Enabled = true;
                buttonStop.Enabled = false;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            m_running = false;
        }
    }
}
