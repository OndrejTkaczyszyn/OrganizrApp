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
    public class ReminderDateDialog : DialogFragment
    {
        public DateTime _workingdate = DateTime.MinValue;
        ViewGroup _container;
        public interface OnNewDatePass
        {
            void onNewDatePass(DateTime date, int id);
            void openTimeDialog(int id, Bundle bundle);
        }

        public OnNewDatePass dataPasser
        {
            get; set;
        }

        public static ReminderDateDialog NewInstance(Bundle bundle)
        {
            ReminderDateDialog fragment = new ReminderDateDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);

            dataPasser = (OnNewDatePass)a;
            Console.WriteLine("activity casted to data passer");
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_date_dialog, container, false);//end potentially true

            Button buttonNext = view.FindViewById<Button>(Resource.Id.DialogButtonNext);
            int day = Arguments.GetInt("day");
            int month = Arguments.GetInt("month");
            int year = Arguments.GetInt("year");

            var DateField = view.FindViewById<CalendarView>(Resource.Id.ActivityEditDateField);
            //DateTime setdate = new DateTime(year,month,day);
            //DateField.Date = setdate.Ticks;
            buttonNext.Click += delegate{
                

                goToNext(view,savedInstanceState);
                Toast.MakeText(Activity, "Dialog fragment dismissed!", ToastLength.Short).Show();
                /*FragmentTransaction ft = FragmentManager.BeginTransaction();
                //Remove fragment else it will crash as it is already added to backstack
                Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                if (prev != null)
                {
                    ft.Remove(prev);
                }

                ft.AddToBackStack(null);*/
                Dialog.Dismiss();

            };
            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            dialog.Dismiss();
        }

        public void goToNext(View view, Bundle savedInstanceState)
        {
            //get data 

            var DateField = view.FindViewById<CalendarView>(Resource.Id.ActivityEditDateField);
            DateField.DateChange += (s, e) =>
            {
                int day = e.DayOfMonth;
                int month = e.Month;
                int year = e.Year;
                _workingdate = new System.DateTime(year, month, day);
                Console.WriteLine("Date:" + day + " / " + month + " / " + year);
            };
            //work with data entered here
            Console.WriteLine("Got data");
            /* int day = DateField.Date.DayOfMonth;
             int month = DateField.Date.Month;
             int year = DateField.Month*/
            int id = Arguments.GetInt("id");
            dataPasser.onNewDatePass(_workingdate,id);
            dataPasser.openTimeDialog(id,savedInstanceState);

        }
    }
}