namespace AplombTech.WMS.MQTT.WindowsService
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
			this.WmsMqttServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.WmsMqttServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// WmsMqttServiceProcessInstaller
			// 
			this.WmsMqttServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.WmsMqttServiceProcessInstaller.Password = null;
			this.WmsMqttServiceProcessInstaller.Username = null;
			// 
			// WmsMqttServiceInstaller
			// 
			this.WmsMqttServiceInstaller.DisplayName = "WMS MQTT Service";
			this.WmsMqttServiceInstaller.ServiceName = "WmsMqttService";
			this.WmsMqttServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WmsMqttServiceProcessInstaller,
            this.WmsMqttServiceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller WmsMqttServiceProcessInstaller;
		private System.ServiceProcess.ServiceInstaller WmsMqttServiceInstaller;
	}
}