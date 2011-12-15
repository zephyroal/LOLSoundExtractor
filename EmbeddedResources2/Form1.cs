using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace EmbeddedResources2
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button load;
		private System.Windows.Forms.PictureBox picture;
		private System.Windows.Forms.Button load2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.load = new System.Windows.Forms.Button();
            this.picture = new System.Windows.Forms.PictureBox();
            this.load2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // load
            // 
            this.load.Location = new System.Drawing.Point(10, 9);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(105, 24);
            this.load.TabIndex = 0;
            this.load.Text = "Load image 1";
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // picture
            // 
            this.picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picture.Location = new System.Drawing.Point(10, 43);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(253, 216);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture.TabIndex = 1;
            this.picture.TabStop = false;
            // 
            // load2
            // 
            this.load2.Location = new System.Drawing.Point(125, 9);
            this.load2.Name = "load2";
            this.load2.Size = new System.Drawing.Size(105, 24);
            this.load2.TabIndex = 1;
            this.load2.Text = "Load image 2";
            this.load2.Click += new System.EventHandler(this.load2_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(272, 267);
            this.Controls.Add(this.load2);
            this.Controls.Add(this.picture);
            this.Controls.Add(this.load);
            this.Name = "Form1";
            this.Text = "Load image from this Assembly";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void load_Click(object sender, System.EventArgs e)
		{
			picture.Image = new Bitmap(typeof(Form1), "images.demo1.gif");
		}

		private void load2_Click(object sender, System.EventArgs e)
		{
			picture.Image = new Bitmap(typeof(Form1), "demo2.gif");
		}
	}
}
