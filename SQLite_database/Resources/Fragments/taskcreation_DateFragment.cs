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
        public interface OnDatePass
        {
            void onDatePass(String name);

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
            var view = inflater.Inflate(Resource.Layout.fragment_datetime, container, false);//end potentially true

            Button buttonFinish = view.FindViewById<Button>(Resource.Id.ButtonFinish);

            buttonFinish.Click += delegate {
                endMyMisery(view);
            }; 
                


            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public void endMyMisery(View view)
        {

            var DateField = view.FindViewById<EditText>(Resource.Id.ActivityDateField);
            String gotData = DateField.Text;
            //work with data entered here
            Console.WriteLine("Got data");
            Console.WriteLine(gotData);
            dataPasser.onDatePass(gotData);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            transaction.Remove(this);
            transaction.AddToBackStack("moved back to original");

            transaction.Commit();

            var viewToShow = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

            viewToShow.Visibility = ViewStates.Visible;

        }
    }
}