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
    public class DateFragment : Fragment
    {
        public int _year, _month,_day;
        public DateTime _workingdate;
        bool _movingToNext = false;
        public interface OnDatePass
        {
            void onDatePass(DateTime datetime);

        }

        public OnDatePass dataPasser
        {
            get; set;
        }

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);

            dataPasser = (OnDatePass)a;
            Console.WriteLine("activity casted to data passer");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_date, container, false);//end potentially true

            Button buttonFinish = view.FindViewById<Button>(Resource.Id.ButtonNextThree);

            buttonFinish.Click += delegate {
                goToNext(view,savedInstanceState);
            };

            var DateField = view.FindViewById<CalendarView>(Resource.Id.ActivityDateField);
            DateField.DateChange += (s, e) =>
            {
                _day = e.DayOfMonth;
                _month = e.Month;
                _year = e.Year;
                _month++;
                Console.WriteLine("Date:" + _day + " / " + _month + " / " + _year);
                _workingdate = new System.DateTime(_year, _month, _day);
            };



            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public void goToNext(View view, Bundle savedInstanceState)
        {
            _movingToNext = true;
            var DateField = view.FindViewById<CalendarView>(Resource.Id.ActivityDateField);
            //work with data entered here
            Console.WriteLine("Got data");

            dataPasser.onDatePass( _workingdate);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            TimeFragment nextFragment = new TimeFragment();

            transaction.Replace(Resource.Id.RootElement,nextFragment);
            transaction.AddToBackStack("moved to next");

            transaction.Commit();
            nextFragment.OnCreate(savedInstanceState);

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