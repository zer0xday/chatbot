using System;
using System.Windows;

namespace GuiClient
{
    public partial class NameDialog : Window
    {
		public NameDialog()
		{
			InitializeComponent();
		}

		private void btnDialogOkClick_Handler(object sender, RoutedEventArgs e)
		{
			if (nameInput.Text.Length > 0)
			{
				DialogResult = true;
			}
		}

		public string BotName
		{
			get => nameInput.Text;
		}
	}
}
