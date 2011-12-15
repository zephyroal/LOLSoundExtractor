using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.IO;

namespace EmbeddedResources1
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private Assembly loadedAssembly = null;

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button loadAssembly;
		private System.Windows.Forms.ListBox resources;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox image;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.Button save;
		private System.Windows.Forms.OpenFileDialog ofd;
		private System.Windows.Forms.SaveFileDialog sfd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.save = new System.Windows.Forms.Button();
            this.loadAssembly = new System.Windows.Forms.Button();
            this.resources = new System.Windows.Forms.ListBox();
            this.splitter = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.image = new System.Windows.Forms.PictureBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.save);
            this.panel1.Controls.Add(this.loadAssembly);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 43);
            this.panel1.TabIndex = 3;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(144, 9);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(96, 24);
            this.save.TabIndex = 2;
            this.save.Text = "Save to file...";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // loadAssembly
            // 
            this.loadAssembly.Location = new System.Drawing.Point(10, 9);
            this.loadAssembly.Name = "loadAssembly";
            this.loadAssembly.Size = new System.Drawing.Size(124, 24);
            this.loadAssembly.TabIndex = 0;
            this.loadAssembly.Text = "Load Assembly...";
            this.loadAssembly.Click += new System.EventHandler(this.loadAssembly_Click);
            // 
            // resources
            // 
            this.resources.Dock = System.Windows.Forms.DockStyle.Left;
            this.resources.IntegralHeight = false;
            this.resources.ItemHeight = 12;
            this.resources.Location = new System.Drawing.Point(0, 43);
            this.resources.Name = "resources";
            this.resources.Size = new System.Drawing.Size(154, 320);
            this.resources.Sorted = true;
            this.resources.TabIndex = 4;
            this.resources.SelectedIndexChanged += new System.EventHandler(this.resources_SelectedIndexChanged);
            // 
            // splitter
            // 
            this.splitter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter.Location = new System.Drawing.Point(154, 43);
            this.splitter.MinExtra = 205;
            this.splitter.MinSize = 128;
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 320);
            this.splitter.TabIndex = 5;
            this.splitter.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.image);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(157, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(315, 320);
            this.panel2.TabIndex = 6;
            // 
            // image
            // 
            this.image.Dock = System.Windows.Forms.DockStyle.Fill;
            this.image.Location = new System.Drawing.Point(0, 0);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(315, 320);
            this.image.TabIndex = 1;
            this.image.TabStop = false;
            // 
            // ofd
            // 
            this.ofd.DefaultExt = "dll";
            this.ofd.Filter = ".NET Assemblies (*.dll;*.exe)|*.dll;*.exe|All Files (*.*)|*.*";
            this.ofd.Title = "Load an assembly";
            // 
            // sfd
            // 
            this.sfd.Filter = "All Files (*.*)|*.*";
            this.sfd.Title = "Save Resource As";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(472, 363);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.resources);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(384, 258);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Embedded Resource Viewer";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void loadAssembly_Click(object sender, System.EventArgs e)
		{
			ofd.FileName = "";

			if( ofd.ShowDialog() == DialogResult.Cancel )
				return;

			try
			{
				loadedAssembly = Assembly.LoadFrom(ofd.FileName);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				return ;
			}

			LoadResources();
		}

		private void LoadResources()
		{
			string [] resourceNames = loadedAssembly.GetManifestResourceNames();

			resources.Items.Clear();

			if( resourceNames.Length > 0 )
			{
				resources.BeginUpdate();

				foreach(string resourceName in resourceNames)
				{
					resources.Items.Add(resourceName);
				}
					
				resources.EndUpdate();
			}

			Image img = image.Image;

			image.Image = null;

			if( img != null )
			{
				img.Dispose();
				img = null;
			}
		}

		private void save_Click(object sender, System.EventArgs e)
		{
			if( resources.SelectedIndex < 0 )
				return ;

			sfd.FileName = (string) resources.SelectedItem;

			if( sfd.ShowDialog() == DialogResult.Cancel )
				return ;

			Stream outFile = sfd.OpenFile();
			Stream inFile = loadedAssembly.GetManifestResourceStream((string) resources.SelectedItem);

			long length = inFile.Length;

			if( length > int.MaxValue )
			{
				MessageBox.Show("Unable to write file in this version, sorry");
				outFile.Close();
				inFile.Close();
			}

			byte [] bytes = new byte[length];

			inFile.Read( bytes, 0, (int) length );
			outFile.Write( bytes, 0, (int) length );

			inFile.Close();
			outFile.Close();
		}

		private void resources_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( resources.SelectedIndex < 0 )
				return ;

			Stream stream = null;

			try
			{
				stream = loadedAssembly.GetManifestResourceStream((string) resources.SelectedItem);

				Image img = Image.FromStream(stream);
				Image oldImage = image.Image;

				image.Image = img;

				if( oldImage != null )
				{
					oldImage.Dispose();
					oldImage = null;
				}
			}
			catch
			{
			}
			finally
			{
				if( stream != null )
					stream.Close();
			}
		}
	}
}
