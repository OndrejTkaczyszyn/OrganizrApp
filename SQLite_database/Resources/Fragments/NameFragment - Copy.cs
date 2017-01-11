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
    public class FragmentParent : Fragment
    {
        Activity _activity;
        public interface OnDataPass
        {
            void onNameGet(String name);
            void onDateGet(String name);
            void onTextGet(String name);
        }

        public OnDataPass dataPasser
        {
            get;set;
        }

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);

            dataPasser = (OnDataPass)a;
            Console.WriteLine("activity casted to data passer");
            this._activity = a;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_name,container,false);//end potentially true

            Button buttonNext = view.FindViewById<Button>(Resource.Id.ButtonNextOne);


            buttonNext.Click += delegate {
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

            
            //get data entered here
            var NameField = view.FindViewById<EditText>(Resource.Id.ActivityNameField);
            String gotData = NameField.Text;
            //work with data entered here
            Console.WriteLine("Got data");
            Console.WriteLine(gotData);
            dataPasser.onNameGet(gotData);


            Console.WriteLine("Button clicked");
            //FragmentTransaction transaction = FragmentManager.BeginTransaction();


            TextFragment secondFragment = new TextFragment();

            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.RootElement, secondFragment, "FRAG_TAG");
            Console.WriteLine("Views Replaced");
            transaction.AddToBackStack(null);
            Console.WriteLine("Transaction added to back stack");
            transaction.Commit();
            Console.WriteLine("Transaction commited");
            secondFragment.OnCreate(savedInstanceState);


            /*FragmentTransaction transactionRemove = FragmentManager.BeginTransaction();

            transaction.Add(Resource.Id.RootElement, secondFragment);
            transaction.Commit();*/
        }
    }
}