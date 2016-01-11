namespace Nes.Debugger
{
    partial class NesDebugger
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.textBoxRegA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxRegP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxRegX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxRegY = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPC = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSP = new System.Windows.Forms.TextBox();
            this.splitContainerHorizontal = new System.Windows.Forms.SplitContainer();
            this.splitContainerVertical = new System.Windows.Forms.SplitContainer();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxCtrl = new System.Windows.Forms.TextBox();
            this.textBoxMask = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxScan = new System.Windows.Forms.TextBox();
            this.textBoxPix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxDisassembly = new System.Windows.Forms.TextBox();
            this.buttonStep = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHorizontal)).BeginInit();
            this.splitContainerHorizontal.Panel1.SuspendLayout();
            this.splitContainerHorizontal.Panel2.SuspendLayout();
            this.splitContainerHorizontal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).BeginInit();
            this.splitContainerVertical.Panel1.SuspendLayout();
            this.splitContainerVertical.Panel2.SuspendLayout();
            this.splitContainerVertical.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLog.Location = new System.Drawing.Point(3, 3);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(729, 224);
            this.textBoxLog.TabIndex = 0;
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(13, 13);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 1;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // textBoxRegA
            // 
            this.textBoxRegA.Location = new System.Drawing.Point(53, 16);
            this.textBoxRegA.Name = "textBoxRegA";
            this.textBoxRegA.Size = new System.Drawing.Size(100, 20);
            this.textBoxRegA.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "RegA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RegP";
            // 
            // textBoxRegP
            // 
            this.textBoxRegP.Location = new System.Drawing.Point(53, 42);
            this.textBoxRegP.Name = "textBoxRegP";
            this.textBoxRegP.Size = new System.Drawing.Size(100, 20);
            this.textBoxRegP.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "RegX";
            // 
            // textBoxRegX
            // 
            this.textBoxRegX.Location = new System.Drawing.Point(53, 68);
            this.textBoxRegX.Name = "textBoxRegX";
            this.textBoxRegX.Size = new System.Drawing.Size(100, 20);
            this.textBoxRegX.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "RegY";
            // 
            // textBoxRegY
            // 
            this.textBoxRegY.Location = new System.Drawing.Point(53, 94);
            this.textBoxRegY.Name = "textBoxRegY";
            this.textBoxRegY.Size = new System.Drawing.Size(100, 20);
            this.textBoxRegY.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "PC";
            // 
            // textBoxPC
            // 
            this.textBoxPC.Location = new System.Drawing.Point(53, 153);
            this.textBoxPC.Name = "textBoxPC";
            this.textBoxPC.Size = new System.Drawing.Size(100, 20);
            this.textBoxPC.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "SP";
            // 
            // textBoxSP
            // 
            this.textBoxSP.Location = new System.Drawing.Point(53, 179);
            this.textBoxSP.Name = "textBoxSP";
            this.textBoxSP.Size = new System.Drawing.Size(100, 20);
            this.textBoxSP.TabIndex = 12;
            // 
            // splitContainerHorizontal
            // 
            this.splitContainerHorizontal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerHorizontal.Location = new System.Drawing.Point(13, 42);
            this.splitContainerHorizontal.Name = "splitContainerHorizontal";
            this.splitContainerHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerHorizontal.Panel1
            // 
            this.splitContainerHorizontal.Panel1.Controls.Add(this.splitContainerVertical);
            // 
            // splitContainerHorizontal.Panel2
            // 
            this.splitContainerHorizontal.Panel2.Controls.Add(this.textBoxLog);
            this.splitContainerHorizontal.Size = new System.Drawing.Size(735, 461);
            this.splitContainerHorizontal.SplitterDistance = 227;
            this.splitContainerHorizontal.TabIndex = 14;
            // 
            // splitContainerVertical
            // 
            this.splitContainerVertical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerVertical.Location = new System.Drawing.Point(3, 3);
            this.splitContainerVertical.Name = "splitContainerVertical";
            // 
            // splitContainerVertical.Panel1
            // 
            this.splitContainerVertical.Panel1.Controls.Add(this.label11);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxCtrl);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxMask);
            this.splitContainerVertical.Panel1.Controls.Add(this.label12);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxStatus);
            this.splitContainerVertical.Panel1.Controls.Add(this.label10);
            this.splitContainerVertical.Panel1.Controls.Add(this.label7);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxScan);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxPix);
            this.splitContainerVertical.Panel1.Controls.Add(this.label8);
            this.splitContainerVertical.Panel1.Controls.Add(this.label1);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxRegY);
            this.splitContainerVertical.Panel1.Controls.Add(this.label6);
            this.splitContainerVertical.Panel1.Controls.Add(this.label3);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxRegA);
            this.splitContainerVertical.Panel1.Controls.Add(this.label4);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxSP);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxRegX);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxRegP);
            this.splitContainerVertical.Panel1.Controls.Add(this.textBoxPC);
            this.splitContainerVertical.Panel1.Controls.Add(this.label5);
            this.splitContainerVertical.Panel1.Controls.Add(this.label2);
            // 
            // splitContainerVertical.Panel2
            // 
            this.splitContainerVertical.Panel2.Controls.Add(this.textBoxDisassembly);
            this.splitContainerVertical.Size = new System.Drawing.Size(729, 221);
            this.splitContainerVertical.SplitterDistance = 362;
            this.splitContainerVertical.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(192, 156);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(22, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Ctrl";
            // 
            // textBoxCtrl
            // 
            this.textBoxCtrl.Location = new System.Drawing.Point(233, 153);
            this.textBoxCtrl.Name = "textBoxCtrl";
            this.textBoxCtrl.Size = new System.Drawing.Size(100, 20);
            this.textBoxCtrl.TabIndex = 22;
            // 
            // textBoxMask
            // 
            this.textBoxMask.Location = new System.Drawing.Point(233, 179);
            this.textBoxMask.Name = "textBoxMask";
            this.textBoxMask.Size = new System.Drawing.Size(100, 20);
            this.textBoxMask.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(192, 182);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Mask";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(233, 127);
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.Size = new System.Drawing.Size(100, 20);
            this.textBoxStatus.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(192, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Status";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(192, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Scan";
            // 
            // textBoxScan
            // 
            this.textBoxScan.Location = new System.Drawing.Point(233, 16);
            this.textBoxScan.Name = "textBoxScan";
            this.textBoxScan.Size = new System.Drawing.Size(100, 20);
            this.textBoxScan.TabIndex = 14;
            // 
            // textBoxPix
            // 
            this.textBoxPix.Location = new System.Drawing.Point(233, 42);
            this.textBoxPix.Name = "textBoxPix";
            this.textBoxPix.Size = new System.Drawing.Size(100, 20);
            this.textBoxPix.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(192, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Pix";
            // 
            // textBoxDisassembly
            // 
            this.textBoxDisassembly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDisassembly.Location = new System.Drawing.Point(3, 3);
            this.textBoxDisassembly.Multiline = true;
            this.textBoxDisassembly.Name = "textBoxDisassembly";
            this.textBoxDisassembly.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDisassembly.Size = new System.Drawing.Size(357, 215);
            this.textBoxDisassembly.TabIndex = 0;
            // 
            // buttonStep
            // 
            this.buttonStep.Location = new System.Drawing.Point(94, 13);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(75, 23);
            this.buttonStep.TabIndex = 15;
            this.buttonStep.Text = "Step";
            this.buttonStep.UseVisualStyleBackColor = true;
            this.buttonStep.Click += new System.EventHandler(this.buttonStep_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(175, 13);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 16;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // NesDebugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 515);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStep);
            this.Controls.Add(this.splitContainerHorizontal);
            this.Controls.Add(this.buttonGo);
            this.Name = "NesDebugger";
            this.Text = "NES Debugger";
            this.splitContainerHorizontal.Panel1.ResumeLayout(false);
            this.splitContainerHorizontal.Panel2.ResumeLayout(false);
            this.splitContainerHorizontal.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHorizontal)).EndInit();
            this.splitContainerHorizontal.ResumeLayout(false);
            this.splitContainerVertical.Panel1.ResumeLayout(false);
            this.splitContainerVertical.Panel1.PerformLayout();
            this.splitContainerVertical.Panel2.ResumeLayout(false);
            this.splitContainerVertical.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).EndInit();
            this.splitContainerVertical.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox textBoxRegA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRegP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRegX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxRegY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSP;
        private System.Windows.Forms.SplitContainer splitContainerHorizontal;
        private System.Windows.Forms.SplitContainer splitContainerVertical;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.TextBox textBoxDisassembly;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxScan;
        private System.Windows.Forms.TextBox textBoxPix;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxCtrl;
        private System.Windows.Forms.TextBox textBoxMask;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonStop;
    }
}

