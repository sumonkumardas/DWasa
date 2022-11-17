namespace AplombTech.WMS.Data.Processor.WindowsService
{
	partial class ProjectInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.WmsDataProcessorServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.WmsDataProcessorServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// WmsDataProcessorServiceProcessInstaller
			// 
			this.WmsDataProcessorServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.WmsDataProcessorServiceProcessInstaller.Password = null;
			this.WmsDataProcessorServiceProcessInstaller.Username = null;
			// 
			// WmsDataProcessorServiceInstaller
			// 
			this.WmsDataProcessorServiceInstaller.DisplayName = "WMS Data Processor Servise";
			this.WmsDataProcessorServiceInstaller.ServiceName = "WmsDataProcessorService";
			this.WmsDataProcessorServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WmsDataProcessorServiceProcessInstaller,
            this.WmsDataProcessorServiceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller WmsDataProcessorServiceProcessInstaller;
		private System.ServiceProcess.ServiceInstaller WmsDataProcessorServiceInstaller;
	}
}