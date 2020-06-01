using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ControlsSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
    {
		private IEnumerable<DataEntry> _data;

		public IEnumerable<DataEntry> Data
		{
			get { return _data; }
			set { _data = value; OnPropertyChanged(nameof(Data)); }
		}
		public MainPage()
        {
            InitializeComponent();

			Data = new List<DataEntry>()
                {
                    new DataEntry()
                    {
                        Percent = 62.7,
                        Label = "CO2",
                    },
                    new DataEntry()
                    {
                        Percent = 29.5,
                        Label = "TVOC",
                    },
                    new DataEntry()
                    {
                        Percent = 85.2,
                        Label = "PM 2.5",
                    },
                    new DataEntry()
                    {
                        Percent = 45.6,
                        Label = "Nox",
                    },
                };
        }
    }
}
