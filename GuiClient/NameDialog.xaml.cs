using System;
using System.Windows;

namespace GuiClient
{
    /// <summary>
    /// Interaction logic for NameDialog.xaml
    /// </summary>
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
