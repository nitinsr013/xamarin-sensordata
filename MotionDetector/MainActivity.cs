using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Content;
using System;
using System.IO;

namespace MotionDetector
{
    [Activity(Label = "MotionDetector", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ISensorEventListener
    {
        static readonly object _syncLock = new object();
        SensorManager _sensorManager;
        TextView _sensorTextView;
        TextView _sensorGyroscopeTextView;
        string accelerometer = string.Empty;
        string gyroscope = string.Empty;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
            _sensorGyroscopeTextView = FindViewById<TextView>(Resource.Id.gyroscope_text);
            Button pauseButton = FindViewById<Button>(Resource.Id.PauseButton);
            Button resumeButton = FindViewById<Button>(Resource.Id.ResumeButton);
            Button shareButton = FindViewById<Button>(Resource.Id.Share);

            pauseButton.Click += (object sender, EventArgs e) =>
            {
                this.OnPause();
            };
            resumeButton.Click += (object sender, EventArgs e) =>
            {
                this.OnResume();
            };
            shareButton.Click += (object sender, EventArgs e) =>
            {
                this.Share();
            };
        }
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // We don't want to do anything here.
        }

        public void OnSensorChanged(SensorEvent e)
        {

            lock (_syncLock)
            {
               
                if (e.Sensor.Type == SensorType.Accelerometer)
                {
                    _sensorTextView.Text = string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
                    accelerometer+= string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
                }
                if (e.Sensor.Type == SensorType.Gyroscope)
                {
                    _sensorGyroscopeTextView.Text = string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
                    gyroscope+= string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
                }
            }


        }
        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this,
                                            _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                                            SensorDelay.Ui);
            _sensorManager.RegisterListener(this,
                                            _sensorManager.GetDefaultSensor(SensorType.Gyroscope),
                                            SensorDelay.Ui);
            
        }
        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        private void Share()
        {
            //int READ_REQUEST_CODE = 42;
            //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //    string filename = Path.Combine(path, "myfile.txt");            
            //    using (var streamWriter = new StreamWriter(filename, true))
            //    {
            //        streamWriter.WriteLine(DateTime.UtcNow);
            //        streamWriter.WriteLine(accelerometer);
            //        streamWriter.WriteLine(gyroscope);               
            //    }
            //   // File.Create(filename);
            //    var intent = new Intent(Intent.ActionSend);
            //    intent.SetType("application/text");

            //    intent.PutExtra(Intent.ExtraStream, filename);           

            //    var intentChooser = Intent.CreateChooser(intent, "Share via");

            //    StartActivityForResult(intentChooser, READ_REQUEST_CODE);
            var intent = new Intent(Intent.ActionSend);
            Intent sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.PutExtra(Intent.ExtraText, DateTime.UtcNow+ "/n"+ "accelerometer: "+ accelerometer+ "/n" + " gyroscope:" + gyroscope);
            sendIntent.SetType("text/plain");
            StartActivity(Intent.CreateChooser(sendIntent, "Send email"));
            accelerometer = string.Empty;
            gyroscope = string.Empty;
        }

       



    }
}

