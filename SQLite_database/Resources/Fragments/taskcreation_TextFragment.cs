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
    public class TextFragment : Fragment
    {

        public interface OnTextPass
        {
            void onTextPass(String name);

        }

        public OnTextPass dataPasser
        {
            get; set;
        }

        bool _movingToNext = false;

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);

            dataPasser = (OnTextPass)a;
            Console.WriteLine("activity casted to data passer");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_addspecs, container, false);//end potentially true

            Button buttonNext = view.FindViewById<Button>(Resource.Id.ButtonNextTwo);

            buttonNext.Click += delegate{
                goToNext(view,savedInstanceState);
            };

            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public void goToNext(View view, Bundle savedInstanceState)
        {
            //get data 
            _movingToNext = true;
            var TextField = view.FindViewById<EditText>(Resource.Id.ActivityTextField);
            String gotData = TextField.Text;
            //work with data entered here
            Console.WriteLine("Got data");
            Console.WriteLine(gotData);
            dataPasser.onTextPass(gotData);
            //end get data
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();

            DateFragment thirdFragment = new DateFragment();
            //firstFragment.Show(transaction,"dialog fragment");            
            transaction.Replace(Resource.Id.RootElement, thirdFragment, "TAG_FRAGMENT");
            thirdFragment.OnCreate(savedInstanceState);
            transaction.AddToBackStack("move to third pane");
            transaction.Commit();

 

        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            if (!_movingToNext)
            {
                var viewToShow = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

                viewToShow.Visibility = ViewStates.Visible;
            }
        }
    }
}