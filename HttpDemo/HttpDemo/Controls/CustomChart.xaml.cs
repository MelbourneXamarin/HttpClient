using System.Collections.Generic;
using System.Collections.Specialized;
using Microcharts;
using Xamarin.Forms;

namespace HttpDemo.Controls
{
    public partial class CustomChart
    {
        public static readonly BindableProperty ChartTypeProperty = BindableProperty.Create(
        nameof(ChartType),
        typeof(Chart),
        typeof(CustomChart),
        default(Chart),
        BindingMode.OneWay,
        propertyChanged: ChartTypeChanged);

        public Chart ChartType
        {
            get { return (Chart)GetValue(ChartTypeProperty); }
            set { SetValue(ChartTypeProperty, value); }
        }

        private static void ChartTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomChart customChart)
            {
                customChart.SetChart((Chart)newValue, customChart.ItemSource);
            }
        }

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(
        nameof(ItemSource),
        typeof(IEnumerable<IEnumerable<Microcharts.Entry>>),
        typeof(CustomChart),
        default(IEnumerable<IEnumerable<Microcharts.Entry>>),
        BindingMode.TwoWay,
        propertyChanged: ItemSoureChanged);

        public IEnumerable<IEnumerable<Microcharts.Entry>> ItemSource
        {
            get { return (IEnumerable<IEnumerable<Microcharts.Entry>>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        private static void ItemSoureChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomChart customChart)
            {
                if (customChart.ItemSource == null)
                    return;
                customChart.SetChart(customChart.ChartType, (IEnumerable<IEnumerable<Microcharts.Entry>>)newValue);
                //var chart = new LineChart() { Entries = entries, LineMode = LineMode.Straight };
                if (newValue is IEnumerable<IEnumerable<Microcharts.Entry>> collection)
                {
                    foreach (var item in collection)
                    {
                        if (item is INotifyCollectionChanged notifyCollection)
                        {
                            notifyCollection.CollectionChanged += (sender, args) =>
                            {
                                customChart.SetChart(customChart.ChartType, (IEnumerable<IEnumerable<Microcharts.Entry>>)newValue);
                            };
                        }
                    }
                }
            }
        }

        private void SetChart(Chart chart, IEnumerable<IEnumerable<Microcharts.Entry>> entries)
        {
            if (chart != null)
            {
                chart.Entries = entries;
                chartControl.Chart = chart;
                chartControl.InvalidateSurface();
            }
        }

        public CustomChart()
        {
            InitializeComponent();
        }
    }
}
