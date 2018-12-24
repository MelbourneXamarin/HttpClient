using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FreshMvvm;
using HttpDemo.Models;
using Microcharts;
using SkiaSharp;
using Xamarin.Essentials;

namespace HttpDemo.ViewModels
{
    public class MainViewModel : FreshBasePageModel
    {
        public bool showMean = false;

        public ISingletonApiService singletonApi { get; }
        public IDisposableApiService disposableApi { get; }

        public static readonly string DisposableGet = "DisposableGet";
        public static readonly string SingletonGet = "SingletonGet";
        public static readonly string DisposablePost = "DisposablePost";
        public static readonly string SingletonPost = "SingletonPost";

        public Chart ChartType { get; set; }
        public ObservableCollection<ObservableCollection<Entry>> ChartData { get; set; }
        public ObservableCollection<Entry> ChartDataTotalDisposable { get; set; }
        public ObservableCollection<Entry> ChartDataTotalSingleton { get; set; }

        public float? LastDisposableTimeGet { get; set; }
        public float? MeanDisposableTimeGet => ChartDataTotalDisposable.Where(c => c.ValueLabel == DisposableGet).Sum(c=>c.Value)/ CountDisposableTimeGet;
        public float CountDisposableTimeGet { get; set; }
        public float? LastDisposableTimePost { get; set; }
        public float? MeanDisposableTimePost => ChartDataTotalDisposable.Where(c => c.ValueLabel == DisposablePost).Sum(c=>c.Value)/CountDisposableTimePost;
        public float CountDisposableTimePost { get; set; }
        public float? LastSingletonTimeGet { get; set; }
        public float? MeanSingletonTimeGet => ChartDataTotalSingleton.Where(c => c.ValueLabel == SingletonGet).Sum(c => c.Value) / CountSingletonTimeGet;
        public float CountSingletonTimeGet { get; set; }
        public float? LastSingletonTimePost { get; set; }
        public float? MeanSingletonTimePost => ChartDataTotalSingleton.Where(c => c.ValueLabel == SingletonPost).Sum(c => c.Value) / CountSingletonTimePost;
        public float CountSingletonTimePost { get; set; }

        Timer timerDisposableGet;
        Timer timerDisposablePost;
        Timer timerSingletonGet;
        Timer timerSingletonPost;

        public MainViewModel(ISingletonApiService singletonApi, IDisposableApiService disposableApi)
        {
            this.disposableApi = disposableApi;
            this.singletonApi = singletonApi;
            ChartType = new LineChart { LineMode = LineMode.Straight };
            ChartData = new ObservableCollection<ObservableCollection<Entry>>
            {
                new ObservableCollection<Entry>(),
                new ObservableCollection<Entry>(),
                new ObservableCollection<Entry>(),
                new ObservableCollection<Entry>(),
                new ObservableCollection<Entry>(),
                new ObservableCollection<Entry>(),
            };
            ChartDataTotalDisposable = new ObservableCollection<Entry>();
            ChartDataTotalSingleton = new ObservableCollection<Entry>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                GetDisposableData();
            }).Start();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                GetSingletonData();
            }).Start();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                PostDisposableData();
            }).Start();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                PostSingletonData();
            }).Start();
        }

        private async Task GetDisposableData()//Red
        {
            while (true)
            {
                timerDisposableGet = new Timer(TimerDisposableGet_Elapsed, null, 1, 1);

                var test = await disposableApi.Get<Connection>("Connection/1");
                Debug.WriteLine($"GetDisposableData:{timeDisposableGet}");
                if (test.Id != "1") throw new Exception();

                FinishGraphData(timeDisposableGet, DisposableGet, SKColor.Parse("#f44141"), timerDisposableGet);
                timeDisposableGet = 0;
            }
        }

        private async Task GetSingletonData()//green
        {
            while (true)
            {
                timerSingletonGet = new Timer(TimerDisposablePost_Elapsed, null, 1, 1);

                var test = await singletonApi.Get<Connection>("Connection/2");
                Debug.WriteLine($"GetSingletonData:{timeSingletonGet}");
                if (test.Id != "2") throw new Exception();
                FinishGraphData(timeSingletonGet, SingletonGet, SKColor.Parse("#90D585"), timerSingletonGet);
                timeSingletonGet = 0;
            }
        }


        private async Task PostDisposableData()//purple
        {
            while (true)
            {
                timerDisposablePost = new Timer(TimerSingletonGet_Elapsed, null, 1, 1);

                await disposableApi.Post<Connection>("Connection", new Connection { CallsForConnection = 1, ConnectionType = ConnectionType.Disposable, Time = 0 });
                Debug.WriteLine($"PostDisposableData: {timeDisposablePost}");

                FinishGraphData(timeDisposablePost, DisposablePost, SKColor.Parse("#f441e5"), timerDisposablePost);
                timeDisposablePost = 0;
            }
        }

        private async Task PostSingletonData()//blue
        {
            while (true)
            {
                timerSingletonPost = new Timer(TimerSingletonPost_Elapsed, null, 1, 1);

                await singletonApi.Post<Connection>("Connection", new Connection { CallsForConnection = 1, ConnectionType = ConnectionType.Singleton, Time = 0 });
                Debug.WriteLine($"PostSingletonData: {timeSingletonPost}");

                FinishGraphData(timeSingletonPost, SingletonPost, SKColor.Parse("#4286f4"), timerSingletonPost);
                timeSingletonPost = 0;
            }
        }

        private void FinishGraphData(int label, string valueLabel, SKColor color, Timer timer)
        {
            timer.Change(
                        Timeout.Infinite,
                        Timeout.Infinite);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var item = new Entry(label) { ValueLabel = valueLabel, Color = color, };

                switch (valueLabel)
                {
                    case nameof(DisposableGet):
                        if (ChartData.ElementAt(0).Count() >= 100)
                            ChartData.ElementAt(0).RemoveAt(0);
                        ChartData.ElementAt(0).Add(item);
                        ChartDataTotalDisposable.Add(item);

                        LastDisposableTimeGet = label;
                        RaisePropertyChanged(nameof(MeanDisposableTimeGet));
                        CountDisposableTimeGet++;

                        MeanLine(ChartData.ElementAt(2), MeanDisposableTimeGet, color);
                        break;
                    case nameof(DisposablePost):
                        if (ChartData.ElementAt(0).Count() >= 100)
                            ChartData.ElementAt(0).RemoveAt(0);
                        ChartData.ElementAt(0).Add(item);
                        ChartDataTotalDisposable.Add(item);

                        LastDisposableTimePost = label;
                        RaisePropertyChanged(nameof(MeanDisposableTimePost));
                        CountDisposableTimePost++;

                        MeanLine(ChartData.ElementAt(3), MeanDisposableTimePost, color);
                        break;
                    case nameof(SingletonGet):
                        if (ChartData.ElementAt(1).Count() >= 100)
                            ChartData.ElementAt(1).RemoveAt(0);
                        ChartData.ElementAt(1).Add(item);
                        ChartDataTotalSingleton.Add(item);

                        LastSingletonTimeGet = label;
                        RaisePropertyChanged(nameof(MeanSingletonTimeGet));
                        CountSingletonTimeGet++;

                        MeanLine(ChartData.ElementAt(4), MeanSingletonTimeGet, color);
                        break;
                    case nameof(SingletonPost):
                        if (ChartData.ElementAt(1).Count() >= 100)
                            ChartData.ElementAt(1).RemoveAt(0);
                        ChartData.ElementAt(1).Add(item);
                        ChartDataTotalSingleton.Add(item);

                        LastSingletonTimePost = label;
                        RaisePropertyChanged(nameof(MeanSingletonTimePost));
                        CountSingletonTimePost++;

                        MeanLine(ChartData.ElementAt(5), MeanSingletonTimePost, color);
                        break;
                }
            });
        }

        private void MeanLine(ObservableCollection<Entry> entries, float? value, SKColor color)
        {
            if (showMean == false) return;
            if (value == null) return;
            var itemAverage = new Entry((float)value) { Color = color, };
            entries.Add(itemAverage);
            if (entries.Count() >= 100)
                entries.RemoveAt(0);
        }

        private static int timeDisposableGet { get; set; }
        private static int timeDisposablePost { get; set; }
        private static int timeSingletonGet { get; set; }
        private static int timeSingletonPost { get; set; }


        static void TimerDisposableGet_Elapsed(object state)
        {
            timeDisposableGet++;
        }
        static void TimerDisposablePost_Elapsed(object state)
        {
            timeDisposablePost++;
        }
        static void TimerSingletonGet_Elapsed(object state)
        {
            timeSingletonGet++;
        }
        static void TimerSingletonPost_Elapsed(object state)
        {
            timeSingletonPost++;
        }
    }
}