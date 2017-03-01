using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace SQLite_database
{
    public class ReminderTimeDialog : DialogFragment
    {
        public DateTime _workingtime = DateTime.MinValue;
        public int _id,_hour,_min;
        ViewGroup _container;
        public interface OnNewTimePass
        {
            void onNewTimePass(DateTime date, int id);
            void closeTimeDialog(ReminderTimeDialog dialog);
        }

        public OnNewTimePass dataPasser
        {
            get; set;
        }

        public static ReminderTimeDialog NewInstance(Bundle bundle)
        {
            ReminderTimeDialog fragment = new ReminderTimeDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);
            Console.WriteLine("Hello world! I am TimeDialog And I have just been attached");
            dataPasser = (OnNewTimePass)a;
            Console.WriteLine("activity casted to data passer");
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_time_dialog, container, false);//end potentially true
            this.SetStyle(DialogFragmentStyle.NoTitle, 0);//TODO: Create own theme and style
            Button buttonNext = view.FindViewById<Button>(Resource.Id.DialogButtonEnd);

            var TimeField = view.FindViewById<TimePicker>(Resource.Id.ActivityEditTimeField);
             int hour = Arguments.GetInt("hour");
             int minute = Arguments.GetInt("minute");
             /*TimeField.Hour = hour;
             TimeField.Minute = minute;*/

            TimeField.TimeChanged += (s, e) =>
            {
                _hour = e.HourOfDay;
                _min = e.Minute;
                _workingtime = new System.DateTime(_workingtime.Year, _workingtime.Month, _workingtime.Day, _hour, _min, 0);
                Console.WriteLine("Time:" + _hour + " / " + _min);
            };



            buttonNext.Click += delegate {
                goToNext(view,savedInstanceState);
               /* FragmentTransaction ft = FragmentManager.BeginTransaction();
                //Remove fragment else it will crash as it is already added to backstack
                Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                if (prev != null)
                {
                    ft.Remove(prev);
                }

                ft.AddToBackStack(null);
                ft.Commit();*/
                Toast.MakeText(Activity, "Dialog fragment dismissed!", ToastLength.Short).Show();
                //Dismiss();

            };
            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }


        public override void OnDismiss(IDialogInterface dialog)
        {

            /* if (mListener != null && !dismissed)
             {
                 dismissed = true;
                 mListener.onFragmentDismissed();
             }
             else
             {
                 Log.Info("sometag", "DialogFragmentDismissed not set");
             }*/
            base.OnDismiss(dialog);
            Console.WriteLine("Time dialog OnDismiss triggered.");
        }


        public void goToNext(View view, Bundle savedInstanceState)
        {
            //get data 

            
            Console.WriteLine("Got data");

            int id = Arguments.GetInt("id");
            _id = Arguments.GetInt("id");
            Console.WriteLine("Sending nudes: Time: " + _workingtime.Hour + " / " + _workingtime.Minute);
            dataPasser.onNewTimePass(_workingtime, _id);
            dataPasser.closeTimeDialog(this);
            Console.WriteLine("gotonext -> making dialog fragment disappear.");
        }
    }
}
