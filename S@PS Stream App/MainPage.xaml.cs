namespace SAPS
{
	public partial class MainPage : ContentPage
	{
		private static MainPage s_Instance;
		public static MainPage Instance
		{
			get
			{
				if (s_Instance == null)
					s_Instance = new MainPage();

				return s_Instance;
			}
		}

		public MainPage()
		{
			s_Instance = this;

			InitializeComponent();
		}

        private void OnSubmitPressed(object sender, EventArgs e)
        {
			Globals.AuthToken = m_AuthToken.Text;
            Globals.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Globals.AuthToken}");
            Application.Current.MainPage = Homepage.Instance;
        }
    }
}
