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
    public class TimeFragment : Fragment
    {
        public int _hour = 0;
        public int _min = 0;
        bool _movingToNext = false;
        public interface OnTimePass
        {
            void onTimePass(int hour, int minute);

        }

        public OnTimePass dataPasser
        {
            get; set;
        }

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);

            dataPasser = (OnTimePass)a;
            Console.WriteLine("activity casted to data passer");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_time, container, false);//end potentially true

            Button buttonFinish = view.FindViewById<Button>(Resource.Id.ButtonFinish);

            buttonFinish.Click += delegate {
                endMyMisery(view);
            };
            var TimeField = view.FindViewById<TimePicker>(Resource.Id.ActivityTimeField);
            TimeField.TimeChanged += (s, e) =>
            {
                _hour = e.HourOfDay;
                _min = e.Minute;

                Console.WriteLine("Time:" + _hour + " / " + _min);
            };


            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public void endMyMisery(View view)
        {


            //work with data entered here
            Console.WriteLine("Got data");
            /* int day = DateField.Date.DayOfMonth;
             int month = DateField.Date.Month;
             int year = DateField.Month*/
            var TimeField = view.FindViewById<TimePicker>(Resource.Id.ActivityTimeField);
            Console.WriteLine("Time:" + _hour + " / " + _min);
            dataPasser.onTimePass(_hour,_min);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            transaction.Remove(this);
            transaction.AddToBackStack("moved back to original");

            transaction.Commit();

            var viewToShow = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

            viewToShow.Visibility = ViewStates.Visible;

        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            //TODO: add safeguards for changing the task throughout
            if (!_movingToNext)
            {
                var viewToShow = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

                viewToShow.Visibility = ViewStates.Visible;
            }
        }
    }
}