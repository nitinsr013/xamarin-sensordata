using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Content;

namespace MotionDetector
{
    [Activity(Label = "MotionDetector", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ISensorEventListener
    {
        static readonly object _syncLock = new object();
        SensorManager _sensorManager;
        TextView _sensorTextView;
        TextView _sensorGyroscopeTextView;        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
            _sensorGyroscopeTextView = FindViewById<TextView>(Resource.Id.gyroscope_text);
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
                }
                if (e.Sensor.Type == SensorType.Gyroscope)
                {
                    _sensorGyroscopeTextView.Text = string.Format("xg={0:f}, yg={1:f}, zg={2:f}", e.Values[0], e.Values[1], e.Values[2]);
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


    }
}

